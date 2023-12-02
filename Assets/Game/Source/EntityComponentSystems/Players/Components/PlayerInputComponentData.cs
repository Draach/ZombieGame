using Unity.Entities;
using Unity.Mathematics;

namespace Game.Source.EntityComponentSystems.Players
{
    public struct PlayerInputComponentData : IComponentData
    {
        public float2 Direction;
    }
}