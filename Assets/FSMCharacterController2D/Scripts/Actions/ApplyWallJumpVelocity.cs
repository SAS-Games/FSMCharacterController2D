using SAS.StateMachineCharacterController2D;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController2D
{
    public class ApplyWallJumpVelocity : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        private WallJumpConfig _wallJumpConfig;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _wallJumpConfig, key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var isTouchingWall = _characterController.WallFront;
            var wallJumpDirection = _characterController.FacingDirection;
            if (isTouchingWall)
                wallJumpDirection = -_characterController.FacingDirection;

            _characterController.SetVelocity(_wallJumpConfig.Velocity, _wallJumpConfig.Angle, wallJumpDirection);
            _characterController.TryFlip(wallJumpDirection);
        }
    }
}