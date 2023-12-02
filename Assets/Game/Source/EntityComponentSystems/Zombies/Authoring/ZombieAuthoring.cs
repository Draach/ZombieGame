using Unity.Entities;
using UnityEngine;

namespace Game.Source.EntityComponentSystems.Zombies
{
    public class ZombieAuthoring : MonoBehaviour
    {
        public class Baker : Baker<ZombieAuthoring>
        {
            public override void Bake(ZombieAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ZombieTag());
            }
        }
    }
}