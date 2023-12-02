using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Source.EntityComponentSystems.Shared
{
    readonly partial struct MovementAspect : IAspect
    {
        private readonly RefRW<LocalTransform> Transform;
        public readonly RefRO<MovementComponentData> MovementComponentData;

        public void Move(float3 direction, float speedMovement, float deltaTime)
        {
            if (math.length(direction) > 0.0f)
                direction = math.normalize(direction);

            Transform.ValueRW = Transform.ValueRW.Translate(direction * speedMovement * deltaTime);
        }

        public void MoveTowards(float deltaTime, float3 playerPos)
        {
            float3 direction = playerPos - Transform.ValueRO.Position;
            direction = math.normalizesafe(direction);
            Transform.ValueRW.Position += direction * deltaTime;
        }

        public void Rotate(float angle)
        {
            Transform.ValueRW = Transform.ValueRO.RotateY(MovementComponentData.ValueRO.SpeedMovement * angle);
        }

        public float3 GetLocalTransformPosition()
        {
            return Transform.ValueRO.Position;
        }
    }
}