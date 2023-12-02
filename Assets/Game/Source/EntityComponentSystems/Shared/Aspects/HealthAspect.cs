using Unity.Entities;

namespace Game.Source.EntityComponentSystems.Shared
{
    readonly partial struct HealthAspect : IAspect
    {
        private readonly RefRW<HealthComponentData> health;

        public void Hurt(int damage)
        {
            health.ValueRW.CurrentHealth -= damage;
        }
    }
}