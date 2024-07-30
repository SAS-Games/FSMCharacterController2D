using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class UnsetKinematic : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.Rigidbody.isKinematic = false;
        }
    }
}