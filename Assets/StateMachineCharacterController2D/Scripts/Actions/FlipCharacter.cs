using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class FlipCharacter : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        [FieldRequiresChild] private InputHandler _inputHandler;


        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.TryFlip(Sign(_inputHandler.HorizontalMoveInput));
        }

        private int Sign(float f)
        {
            if (f > 0) return 1;
            if (f < 0) return -1;
            return 0;
        }
    }
}