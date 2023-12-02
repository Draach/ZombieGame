using Unity.Entities;

namespace Game.Source.EntityComponentSystems.Shared
{
    public struct HealthComponentData : IComponentData
    {
        public int MaxHealth;
        public int CurrentHealth;
    }
}