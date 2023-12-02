using Game.Source.EntityComponentSystems.Common;
using Game.Source.EntityComponentSystems.Shared;
using Game.Source.EntityComponentSystems.Zombies;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Game.Source.EntityComponentSystems.ZombiesSpawner
{
    public partial struct ZombieSpawnerSystem : ISystem
    {
        private uint updateCounter;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ZombiesSpawnerComponentData>();
            state.RequireForUpdate<Prefabs>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityQuery zombies = SystemAPI.QueryBuilder().WithAspect<CharacterAspect>().WithAll<ZombieTag>().Build();

            if (zombies.IsEmpty)
            {
                ZombiesSpawnerComponentData zombiesSpawnerComponentData = SystemAPI.GetSingleton<ZombiesSpawnerComponentData>();
                Entity zombieEntity = zombiesSpawnerComponentData.ZombieEntity;

                // Instantiate method returns a NativeArray of EntityIds, and as we only need those
                // In the context of this method, we allocate then in a temporary allocator.
                NativeArray<Entity> instances =
                    state.EntityManager.Instantiate(zombieEntity, zombiesSpawnerComponentData.ZombiesCount, Allocator.Temp);
                Random random = Random.CreateFromIndex(updateCounter++);

                foreach (Entity entity in instances)
                {
                    RefRW<LocalTransform> transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                    transform.ValueRW.Position = new float3(random.NextFloat(), 0f, random.NextFloat()) * 20;
                }
            }
        }
    }
}