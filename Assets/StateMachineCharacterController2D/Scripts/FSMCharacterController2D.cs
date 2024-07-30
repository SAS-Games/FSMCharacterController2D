using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Actor), typeof(BoxCollider2D))]
    public partial class FSMCharacterController2D : MonoBehaviour
    {
        public CharacterConfig m_CharacterConfig;

        [field: FieldRequiresSelf] public Rigidbody2D Rigidbody { get; private set; }
        [field: FieldRequiresSelf] public BoxCollider2D BodyCollider { get; private set; }
        [FieldRequiresSelf] private Actor _actor;

        [SerializeField] public PlayerData playerData;
        [SerializeField] private RayCast m_StepLowerCheck;
        [SerializeField] private RayCast m_StepUpperCheck;
        [SerializeField] private BaseCast m_CeilingCheck;

        [SerializeField] private RayCast m_WallCheck;
        [SerializeField] private RayCast m_LedgeCheckHorizontal;
        [SerializeField] private RayCast m_LedgeCheckVertical;

        private int HorizontalSpeedHash = Animator.StringToHash("Speed");
        private int HorizontalInputHash = Animator.StringToHash("HorizontalInput");
        private int VerticalInputHash = Animator.StringToHash("VerticalInput");


        internal bool startCoyoteTimer = false;
        internal bool wallJumpCoyoteTime;
        internal int amountOfJumpsLeft;
        internal Vector2 edgeClimbedStopPosition;
        internal Vector3 wallGrabPosition;

        public bool Ground => HasGroundDetected();
        public bool Ceiling => m_CeilingCheck.IsDetected();
        public bool WallFront => m_WallCheck.IsDetected(Vector2.right * FacingDirection);
        public bool WallBack => m_WallCheck.IsDetected(Vector2.right * -FacingDirection);
        public bool LedgeHorizontal => m_LedgeCheckHorizontal.IsDetected(Vector2.right * FacingDirection);
        public bool LedgeVertical => m_LedgeCheckHorizontal.IsDetected(Vector2.down);

        public bool canSetVelocity;
        internal Vector2 movementVector;
        public int FacingDirection { get; private set; }
        public Vector2 CurrentVelocity => Rigidbody.velocity;

        internal CharacterSize characterSize;
        private const float SKIN_WIDTH = 0.02f;

        public Actor Actor
        {
            get
            {
                if (_actor == null)
                    _actor = GetComponent<Actor>();
                return _actor;
            }
        }

        public Vector2 Up { get; private set; }
        public Vector2 Right { get; private set; }



        private void Awake()
        {
            Application.targetFrameRate = 30;
            this.Initialize();
            characterSize = m_CharacterConfig.GenerateCharacterSize();
            BodyCollider.edgeRadius = 0.05f;
            SetColliderMode(ColliderMode.Standard);
            FacingDirection = 1;
            canSetVelocity = true;
            amountOfJumpsLeft = _actor.Get<JumpData>().AmountOfJumps;
        }
        public Vector2 _immediateMove;
        private void FixedUpdate()
        {
            position = CalculateRayPoint();
            if (_immediateMove != Vector2.zero)
                Rigidbody.MovePosition(Rigidbody.position + _immediateMove);

            var rot = Rigidbody.rotation * Mathf.Deg2Rad;
            Up = new Vector2(-Mathf.Sin(rot), Mathf.Cos(rot));
            Right = new Vector2(Up.y, -Up.x);

            if (canSetVelocity)
            {
                Rigidbody.velocity = movementVector;
                movementVector = Rigidbody.velocity;
            }
            _immediateMove = Vector2.zero;
        }

        public void SetVelocityZero()
        {
            movementVector = Vector2.zero;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            movementVector.Set(angle.x * velocity * direction, angle.y * velocity);
        }

        public void SetVelocity(float velocity, Vector2 direction)
        {
            movementVector = direction * velocity;
        }

        public void SetVelocity(Vector2 velocity)
        {
            movementVector.Set(velocity.x, velocity.y);
        }

        public void SetVelocityY(float velocity)
        {
            movementVector.Set(movementVector.x, velocity);
        }

        public void TryFlip(int direction)
        {
            if (direction != 0 && direction != FacingDirection)
                Flip();
        }

        public void Flip()
        {
            FacingDirection *= -1;
            Rigidbody.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public void OnMove(float normalizedMoveInput)
        {
            Actor.SetFloat(HorizontalSpeedHash, (float)Math.Round(normalizedMoveInput, 2));
        }

        public bool IsFacingInputDirection(float horizontalInput)
        {
            int direction = Sign(horizontalInput);
            if (direction == 0) return false;

            return direction == FacingDirection;
        }

        public int Sign(float f)
        {
            if (f > 0) return 1;
            if (f < 0) return -1;
            return 0;
        }

        public void OnJumpInitiated()
        {
            Actor.SetTrigger("Jump");
            Actor.SetBool("JumpHold", true);
        }

        public void OnJumpCanceled()
        {
            Actor.SetBool("JumpHold", false);
            Actor.ResetSetTrigger("Jump");
        }

        public void OnGrab(bool started)
        {
            Actor.SetBool("Grab", started);
        }

        public void UserInput(Vector2 input)
        {
            Actor.SetFloat(HorizontalInputHash, input.x);
            Actor.SetFloat(VerticalInputHash, input.y);
        }

        internal Vector2 _forceToApplyThisFrame;

        public void AddFrameForce(Vector2 force, bool resetVelocity = false)
        {
            if (resetVelocity)
            {
                Rigidbody.velocity = Vector2.zero;
                movementVector = Vector2.zero;
            }
            SetVelocity(force);
        }

        private void OnValidate()
        {
            // GetComponent<Rigidbody2D>().hideFlags = HideFlags.NotEditable;
            GetComponent<BoxCollider2D>().hideFlags = HideFlags.None;
        }
        public Vector2 position { get; private set; }
        public bool HasGroundDetected()
        {
            // Is the middle ray good?
            var isGroundedThisFrame = PerformRay(position);

            // If not, zigzag rays from the center outward until we find a hit
            if (!isGroundedThisFrame)
            {
                foreach (var offset in GenerateRayOffsets())
                {
                    isGroundedThisFrame = PerformRay(position + Right * offset) || PerformRay(position - Right * offset);
                    if (isGroundedThisFrame) return true;
                }
                return false;
            }
            return true;

            bool PerformRay(Vector2 point)
            {
                var groundHit = Physics2D.Raycast(point, -Up, SKIN_WIDTH, m_CharacterConfig.CollisionLayers);
                if (!groundHit)
                    return false;

                if (Vector2.Angle(groundHit.normal, Up) > m_CharacterConfig.SlopeLimit)
                    return false;

                return true;
            }
        }
        private IEnumerable<float> GenerateRayOffsets()
        {
            int RAY_SIDE_COUNT = 1;
            var extent = characterSize.StandingColliderSize.x / 2 - characterSize.RayInset;
            var offsetAmount = extent / RAY_SIDE_COUNT;
            for (var i = 1; i < RAY_SIDE_COUNT + 1; i++)
            {
                yield return offsetAmount * i;
            }
        }

        private void SetColliderMode(ColliderMode mode)
        {
            switch (mode)
            {
                case ColliderMode.Standard:
                    BodyCollider.size = characterSize.StandingColliderSize;
                    BodyCollider.offset = characterSize.StandingColliderCenter;
                    break;
                case ColliderMode.Crouching:
                    BodyCollider.size = characterSize.CrouchColliderSize;
                    BodyCollider.offset = characterSize.CrouchingColliderCenter;
                    break;
                case ColliderMode.Airborne:
                    break;
            }
        }

        Vector2 CalculateRayPoint()
        {
            Vector2 bodyCenter = BodyCollider.bounds.center;
            float offset = BodyCollider.bounds.size.y * 0.5f + BodyCollider.edgeRadius;
            return bodyCenter - Up * offset;
        }

        public bool IsDetected(out float height)
        {
            Vector2 dir = Vector2.right * FacingDirection;
            float distance = BodyCollider.bounds.size.x * 0.5f + BodyCollider.edgeRadius + SKIN_WIDTH;

            // Bottom ray
            RaycastHit2D hitBottom = Physics2D.Raycast(position, dir, distance, m_CharacterConfig.CollisionLayers);
            height = 0;

            if (hitBottom.collider != null)
            {
                Vector2 upperRayOrigin = position + Up * characterSize.StepOffset;
                RaycastHit2D hitUpper = Physics2D.Raycast(upperRayOrigin, dir, distance + 0.1f, m_CharacterConfig.CollisionLayers);

                if (hitUpper.collider == null)
                {
                    // Calculate the y position for the downward ray
                    float downwardRayY = upperRayOrigin.y;
                    Vector2 downwardRayOrigin = new Vector2(hitBottom.point.x + FacingDirection * 0.1f, downwardRayY);

                    // Cast the downward ray
                    RaycastHit2D hitDownward = Physics2D.Raycast(downwardRayOrigin, Vector2.down, characterSize.StepOffset, m_CharacterConfig.CollisionLayers);

                    if (hitDownward.collider != null)
                    {
                        height = Math.Abs(position.y - hitDownward.point.y);
                        return true;
                    }
                }
            }

            return false;
        }


        private enum ColliderMode
        {
            Standard,
            Crouching,
            Airborne
        }
    }
}