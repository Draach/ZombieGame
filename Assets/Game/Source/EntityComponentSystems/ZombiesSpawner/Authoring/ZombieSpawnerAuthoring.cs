using Unity.Entities;
using UnityEngine;

namespace Game.Source.EntityComponentSystems.ZombiesSpawner
{
    public class ZombieSpawnerAuthoring : MonoBehaviour
    {
        public GameObject ZombiePrefab;
        public int ZombiesCount;

        class Baker : Baker<ZombieSpawnerAuthoring>
        {
            public override void Bake(ZombieSpawnerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ZombiesSpawnerComponentData
                {
                    // We retrieve the baked Entity form of the Prefab assigned in the Authoring component.
                    ZombieEntity = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.None),
                    ZombiesCount = authoring.ZombiesCount,
                });
            }
        }
    }
}
