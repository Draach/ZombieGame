using Game.Source.EntityComponentSystems.Shared;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Source.EntityComponentSystems.Players
{
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (movementAspect, playerInputComponent) in SystemAPI
                         .Query<MovementAspect, RefRO<PlayerInputComponentData>>().WithAll<PlayerTag>())
            {
                float3 moveDirection = new float3(playerInputComponent.ValueRO.Direction.x, 0,
                    playerInputComponent.ValueRO.Direction.y);
                movementAspect.Move(moveDirection, movementAspect.MovementComponentData.ValueRO.SpeedMovement,
                    deltaTime);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}