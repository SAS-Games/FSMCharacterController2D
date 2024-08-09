using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class IsWithinWallJumpCoyoteTime : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
        private JumpData _jumpData = default;
        private float startWallJumpCoyoteTime;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingWallBack;
        private bool oldIsTouchingWall;
        private bool oldIsTouchingWallBack;
        private bool isTouchingLedge;
        private bool wallJumpCoyoteTime;

        public void OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            actor.TryGet(out _jumpData);
        }

        public void OnStateEnter()
        {
        }

        public void OnStateExit()
        {
            oldIsTouchingWall = false;
            oldIsTouchingWallBack = false;

            isTouchingWall = false;
            isTouchingWallBack = false;
            wallJumpCoyoteTime = false;
        }

        public bool Evaluate()
        {
            oldIsTouchingWall = isTouchingWall;
            oldIsTouchingWallBack = isTouchingWallBack;

            isTouchingWall = _characterController.WallFront;
            isTouchingWallBack = _characterController.WallBack;

            if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
            {
                wallJumpCoyoteTime = true;
                startWallJumpCoyoteTime = Time.time;
            }

            CheckWallJumpCoyoteTime();

            return wallJumpCoyoteTime;
        }

        private void CheckWallJumpCoyoteTime()
        {
            if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + _jumpData.CoyoteTime)
            {
                wallJumpCoyoteTime = false;
            }
        }
    }
}