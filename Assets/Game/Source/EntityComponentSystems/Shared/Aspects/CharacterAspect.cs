using Unity.Entities;
using Unity.Mathematics;

namespace Game.Source.EntityComponentSystems.Shared
{
    public readonly partial struct CharacterAspect : IAspect
    {
        // private readonly Entity Self;

        private readonly HealthAspect healthAspect;
        private readonly MovementAspect movementAspect;
        public RefRO<MovementComponentData> Speed => movementAspect.MovementComponentData;

        public void Hurt(int damage)
        {
            healthAspect.Hurt(damage);
        }

        public void Move(float3 direction, float speedMovement, float deltaTime)
        {
            movementAspect.Move(direction, speedMovement, deltaTime);
        }

        public void MoveTowards(float deltaTime, float3 playerPos)
        {
            movementAspect.MoveTowards(deltaTime, playerPos);
        }

        public void Rotate(float deltaTime)
        {
            movementAspect.Rotate(deltaTime);
        }

        public float3 GetLocalTransformPosition()
        {
            return movementAspect.GetLocalTransformPosition();
        }
    }
}