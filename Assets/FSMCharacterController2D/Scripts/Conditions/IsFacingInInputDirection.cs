using SAS.StateMachineCharacterController2D;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

public class IsFacingInInputDirection : ICustomCondition
{
    [FieldRequiresChild] private FSMCharacterController2D _characterController;
    [FieldRequiresChild] private InputHandler _inputHandler;

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
        return _characterController.IsFacingInputDirection(_inputHandler.InputVector.x);
    }
}