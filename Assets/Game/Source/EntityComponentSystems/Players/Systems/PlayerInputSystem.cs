using Settings.Inputs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Source.EntityComponentSystems.Players
{
    public partial class PlayerInputSystem : SystemBase
    {
        private InputControls inputControls;

        protected override void OnCreate()
        {
            inputControls = new InputControls();
        }

        protected override void OnStartRunning()
        {
            inputControls.Character.Enable();
        }

        protected override void OnUpdate()
        {
            Vector2 direction = inputControls.Character.Walk.ReadValue<Vector2>();
            foreach (var playerInputComponent in SystemAPI.Query<RefRW<PlayerInputComponentData>>().WithAll<PlayerTag>())
            {
                playerInputComponent.ValueRW.Direction = new float2(direction.x, direction.y);
            }
        }

        protected override void OnStopRunning()
        {
            inputControls.Character.Disable();
        }

        protected override void OnDestroy()
        {
        }
    }
}