using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class IsGrounded : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController2D _player;

        public void OnInitialize(Actor actor)
        {
            actor.Initialize(this);
        }

        public void OnStateEnter()
        {
        }

        public void OnStateExit()
        {
        }

        public bool Evaluate()
        {
            return _player.Ground;
        }
    }
}