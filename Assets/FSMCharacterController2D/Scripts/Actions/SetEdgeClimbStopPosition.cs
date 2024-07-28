using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class SetEdgeClimbStopPosition : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        private Vector2 _holdPosition;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.transform.position = _characterController.edgeClimbedStopPosition;
        }
    }
}