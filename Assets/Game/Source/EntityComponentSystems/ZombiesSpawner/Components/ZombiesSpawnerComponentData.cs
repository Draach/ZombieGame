using Unity.Entities;

namespace Game.Source.EntityComponentSystems.ZombiesSpawner
{
    struct ZombiesSpawnerComponentData : IComponentData
    {
        // The Entity form of the Authoring Prefab field.
        public Entity ZombieEntity;
        public int ZombiesCount;
    }
}