using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class UpwardMovement : IStateAction
    {
        private FSMCharacterController2D _characterController;
        private UpwardMovementConfig _upwardMovementConfig = default;

        private float _gravityContributionMultiplier;
        private float _verticalMovement;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGet(out _upwardMovementConfig, key);
            actor.TryGetComponent(out _characterController);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _gravityContributionMultiplier = 0;
                _verticalMovement = _upwardMovementConfig.jumpForce;
                _characterController.currentStepDownLength = 0; //todo: note sure it should be here or it should be an action
                return;
            }
            _gravityContributionMultiplier += _upwardMovementConfig.gravityComebackMultiplier;
            _gravityContributionMultiplier *= _upwardMovementConfig.gravityDivider; //Reduce the gravity effect
            _verticalMovement += Physics2D.gravity.y * _upwardMovementConfig.gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier;

            _characterController.SetVelocityY(_verticalMovement);
        }
    }
}
