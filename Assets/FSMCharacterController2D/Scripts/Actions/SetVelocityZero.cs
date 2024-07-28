using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class SetVelocityZero : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _character;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _character.SetVelocityZero();
        }
    }
}