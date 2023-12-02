using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Source.EntityComponentSystems.Players
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new PlayerInputComponentData
                {
                    Direction = float2.zero,
                });
                AddComponent(entity, new PlayerTag());
            }
        }
    }
}