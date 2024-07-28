using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class IsTouchingWall : ICustomCondition
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
            return _characterController.WallFront || _characterController.WallBack;
        }
    }
}