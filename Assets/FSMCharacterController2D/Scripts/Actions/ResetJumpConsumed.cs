using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class ResetJumpConsumed : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        private JumpData _jumpData = default;


        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _jumpData, key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.amountOfJumpsLeft = _jumpData.AmountOfJumps;
        }
    }
}