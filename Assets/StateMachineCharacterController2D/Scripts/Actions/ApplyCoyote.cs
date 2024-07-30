using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class ApplyCoyote : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        private JumpData _jumpData = default;

        private float startTime;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _jumpData, key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                startTime = Time.time;
                return;
            }

            if (_characterController.startCoyoteTimer && Time.time > startTime + _jumpData.CoyoteTime)
            {
                _characterController.startCoyoteTimer = false;
                if (_characterController.amountOfJumpsLeft == _jumpData.AmountOfJumps)
                    _characterController.amountOfJumpsLeft--;
            }
        }
    }
}