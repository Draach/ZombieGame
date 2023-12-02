using Unity.Entities;
using UnityEngine;

namespace Game.Source.EntityComponentSystems.Common
{
    public class ExecuteAuthoring : MonoBehaviour
    {
        public bool MainThread;
        public bool JobEntity;
        public bool Aspects;
        public bool Prefabs;
        public bool JobChunk;
        public bool Reparenting;
        public bool EnableableComponents;
        public bool GameObjectSync;

        class Baker : Baker<ExecuteAuthoring>
        {
            public override void Bake(ExecuteAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                if (authoring.MainThread) AddComponent<MainThread>(entity);
                if (authoring.JobEntity) AddComponent<JobEntity>(entity);
                if (authoring.Aspects) AddComponent<Aspects>(entity);
                if (authoring.Prefabs) AddComponent<Prefabs>(entity);
                if (authoring.JobChunk) AddComponent<JobChunk>(entity);
                if (authoring.GameObjectSync) AddComponent<GameObjectSync>(entity);
                if (authoring.Reparenting) AddComponent<Reparenting>(entity);
                if (authoring.EnableableComponents) AddComponent<EnableableComponents>(entity);
            }
        }
    }

    public struct MainThread : IComponentData
    {
    }

    public struct JobEntity : IComponentData
    {
    }

    public struct Aspects : IComponentData
    {
    }

    public struct Prefabs : IComponentData
    {
    }

    public struct JobChunk : IComponentData
    {
    }

    public struct GameObjectSync : IComponentData
    {
    }

    public struct Reparenting : IComponentData
    {
    }

    public struct EnableableComponents : IComponentData
    {
    }
}