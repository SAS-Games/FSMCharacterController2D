using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class ApplyHorizontalMovement : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        [FieldRequiresChild] private InputHandler _inputHandler;
        private ScriptableReadOnlyFloat _speed;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _speed, key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.movementVector.x = _speed.value * _inputHandler.HorizontalMoveInput;
        }
    }
}