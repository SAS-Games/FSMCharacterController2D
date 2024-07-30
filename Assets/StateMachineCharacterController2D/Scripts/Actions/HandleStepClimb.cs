using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class HandleStepClimb : IStateAction
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
            if (_characterController.IsDetected(out var height))
            {
                _characterController._immediateMove.x = _characterController.characterSize.Width * 0.5f * _characterController.FacingDirection;
                _characterController._immediateMove.y = height;
            }
        }
    }
}
