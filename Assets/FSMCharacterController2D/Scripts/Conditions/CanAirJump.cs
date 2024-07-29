using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class CanAirJump : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
        }

        bool ICustomCondition.Evaluate()
        {
            return _characterController.amountOfJumpsLeft > 0;
        }



        void ICustomCondition.OnStateEnter()
        {
        }

        void ICustomCondition.OnStateExit()
        {
        }
    }
}
