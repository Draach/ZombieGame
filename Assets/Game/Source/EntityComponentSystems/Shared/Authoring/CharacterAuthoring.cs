using Unity.Entities;
using UnityEngine;

namespace Game.Source.EntityComponentSystems.Shared
{
    public class CharacterAuthoring : MonoBehaviour
    {
        public int MaxHealth = 100;
        public int CurrentHealth = 100;
        [Range(1f, 5f)] public float SpeedMovement = 1f;

        public class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealthComponentData()
                {
                    MaxHealth = authoring.MaxHealth,
                    CurrentHealth = authoring.CurrentHealth
                });
                AddComponent(entity, new MovementComponentData
                {
                    SpeedMovement = authoring.SpeedMovement,
                });
            }
        }
    }
}