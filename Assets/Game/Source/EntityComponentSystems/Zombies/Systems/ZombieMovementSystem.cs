using System.Collections.Generic;
using Game.Source.EntityComponentSystems.Common;
using Game.Source.EntityComponentSystems.Players;
using Game.Source.EntityComponentSystems.Shared;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Source.EntityComponentSystems.Zombies
{
    public partial struct ZombieMovementSystem : ISystem
    {
        private NativeArray<float3> playersPositions;
        private NativeArray<float3> zombiesPositions;
        private NativeArray<float3> nearestPlayerPos;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<MainThread>();
        }

        // TODO: Review Allocators, ¿Should those be Allocator.TempJob?
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            NativeArray<Entity> playerEntities = SystemAPI.QueryBuilder().WithAspect<CharacterAspect>()
                .WithAll<PlayerTag>()
                .Build().ToEntityArray(Allocator.Temp);
            playersPositions = new NativeArray<float3>(playerEntities.Length, Allocator.Persistent);

            for (int i = 0; i < playerEntities.Length; i++)
            {
                CharacterAspect charAspect = SystemAPI.GetAspect<CharacterAspect>(playerEntities[i]);
                playersPositions[i] = charAspect.GetLocalTransformPosition();
            }

            NativeArray<Entity> zombiesEntities = SystemAPI.QueryBuilder().WithAspect<CharacterAspect>()
                .WithAll<ZombieTag>()
                .Build().ToEntityArray(Allocator.Temp);
            zombiesPositions = new NativeArray<float3>(zombiesEntities.Length, Allocator.Persistent);
            for (int i = 0; i < zombiesEntities.Length; i++)
            {
                CharacterAspect charAspect = SystemAPI.GetAspect<CharacterAspect>(zombiesEntities[i]);
                zombiesPositions[i] = charAspect.GetLocalTransformPosition();
            }

            nearestPlayerPos = new NativeArray<float3>(zombiesEntities.Length, Allocator.Persistent);

            FindNearestPlayerJob jobHandle = new FindNearestPlayerJob()
            {
                PlayersPositions = playersPositions,
                ZombiesPositions = zombiesPositions,
                NearestPlayerPos = nearestPlayerPos,
            };

            jobHandle.Schedule(zombiesEntities.Length, 1000).Complete();

            int iterator = 0;
            foreach (var characterAspect in SystemAPI.Query<CharacterAspect>().WithAll<ZombieTag>())
            {
                float distance = math.distance(characterAspect.GetLocalTransformPosition(), nearestPlayerPos[iterator]);
                if (distance < 10f)
                {
                    characterAspect.MoveTowards(deltaTime, nearestPlayerPos[iterator]);
                    characterAspect.Rotate(deltaTime);
                    //characterAspect.Hurt(1);
                }

                iterator++;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            playersPositions.Dispose();
            zombiesPositions.Dispose();
            nearestPlayerPos.Dispose();
        }
    }

    [BurstCompile]
    public struct FindNearestPlayerJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> PlayersPositions;
        [ReadOnly] public NativeArray<float3> ZombiesPositions;
        public NativeArray<float3> NearestPlayerPos;

        public void Execute(int index)
        {
            float3 zombiePos = ZombiesPositions[index];

            // Find the target with the closest X coord.
            int startIdx = PlayersPositions.BinarySearch(zombiePos, new AxisXComparer { });

            // When no precise match is found, BinarySearch returns the bitwise negation of the last-searched offset.
            // So when startIdx is negative, we flip the bits again, but we then must ensure the index is within bounds.
            if (startIdx < 0) startIdx = ~startIdx;
            if (startIdx >= PlayersPositions.Length) startIdx = PlayersPositions.Length - 1;

            // The position of the target with the closest X coord.
            float3 nearestTargetPos = PlayersPositions[startIdx];
            float nearestDistSq = math.distancesq(zombiePos, nearestTargetPos);

            // Searching upwards through the array for a closer target.
            Search(zombiePos, startIdx + 1, PlayersPositions.Length, +1, ref nearestTargetPos, ref nearestDistSq);

            // Search downwards through the array for a closer target.
            Search(zombiePos, startIdx - 1, -1, -1, ref nearestTargetPos, ref nearestDistSq);

            NearestPlayerPos[index] = nearestTargetPos;
        }

        void Search(float3 seekerPos, int startIdx, int endIdx, int step,
            ref float3 nearestTargetPos, ref float nearestDistSq)
        {
            for (int i = startIdx; i != endIdx; i += step)
            {
                float3 targetPos = PlayersPositions[i];
                float xdiff = seekerPos.x - targetPos.x;

                // If the square of the x distance is greater than the current nearest, we can stop searching.
                if ((xdiff * xdiff) > nearestDistSq) break;

                float distSq = math.distancesq(targetPos, seekerPos);

                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    nearestTargetPos = targetPos;
                }
            }
        }
    }

    public struct AxisXComparer : IComparer<float3>
    {
        public int Compare(float3 a, float3 b)
        {
            return a.x.CompareTo(b.x);
        }
    }
}