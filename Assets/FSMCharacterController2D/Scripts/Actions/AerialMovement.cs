using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class AerialMovement : IStateAction
    {
        private AerialMovementConfig _aerialMovementConfig;
        [FieldRequiresChild] private FSMCharacterController2D _characterController;
        [FieldRequiresChild] private InputHandler _inputHandler;


        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _aerialMovementConfig);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            Vector2 velocity = _characterController.CurrentVelocity;
            float input = _inputHandler.HorizontalMoveInput;

            SetVelocityPerAxis(ref velocity.x, input, _aerialMovementConfig.acceleration, _aerialMovementConfig.speed);

            _characterController.SetVelocity(velocity);
        }

        private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
        {
            if (axisInput == 0f)
            {
                if (currentAxisSpeed != 0f)
                    ApplyAirResistance(ref currentAxisSpeed);
            }
            else
            {
                (float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
                (float signVel, float signInput) = (Mathf.Sign(currentAxisSpeed), Mathf.Sign(axisInput));
                targetSpeed *= absInput;

                if (signVel != signInput || absVel < targetSpeed)
                {
                    currentAxisSpeed += axisInput * acceleration;
                    currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);
                }
                else
                    ApplyAirResistance(ref currentAxisSpeed);
            }
        }

        private void ApplyAirResistance(ref float value)
        {
            float sign = Mathf.Sign(value);

            value -= sign * _aerialMovementConfig.airResistance * Time.deltaTime;
            if (Mathf.Sign(value) != sign)
                value = 0;
        }
    }
}