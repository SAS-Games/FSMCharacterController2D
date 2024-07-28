using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class ApplyWallClimbVelocity : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        private ScriptableReadOnlyFloat _speed;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _speed, key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.SetVelocityY(_speed.value);
        }
    }
}