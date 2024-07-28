using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class IsTouchingWallBack : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;

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
            return _characterController.WallBack;
        }
    }
}