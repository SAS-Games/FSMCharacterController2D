using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController2D
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputConfig m_InputConfig;
        [SerializeField] private float m_targetSpeedReachMultiplier = 10;
        [SerializeField] private float m_MoveInputScale = 0.6f;
        private Transform _cameraTransform;
        public Vector2 InputVector { get; private set; }
        private float _previousSpeed;
        private FSMCharacterController2D _characterController;
        private float _targetValue;

        Action<CallbackContext> _jumpPerformed;
        Action<CallbackContext> _jumpCanceled;

        Action<CallbackContext> _runStarted;
        Action<CallbackContext> _runCanceled;

        Action<CallbackContext> _grabPerformed;
        Action<CallbackContext> _grabCanceled;

        void Awake()
        {
            _targetValue = m_MoveInputScale;
            _characterController = GetComponent<FSMCharacterController2D>();
            _cameraTransform = Camera.main.transform;
        }

        void OnEnable()
        {
            var moveInputAction = m_InputConfig.GetInputAction("Move");
            moveInputAction.Enable();
            moveInputAction.started += ParseMovementInput;
            moveInputAction.performed += ParseMovementInput;
            moveInputAction.canceled += ParseMovementInput;

            var jumpInputAction = m_InputConfig.GetInputAction("Jump");
            jumpInputAction.Enable();

            _jumpPerformed = _ => _characterController.OnJumpInitiated();
            _jumpCanceled = _ => _characterController.OnJumpCanceled();

            jumpInputAction.performed += _jumpPerformed;
            jumpInputAction.canceled += _jumpCanceled;


            var grabInputAction = m_InputConfig.GetInputAction("Grab");
            grabInputAction.Enable();

            _grabPerformed = _ => _characterController.OnGrab(true);
            _grabCanceled = _ => _characterController.OnGrab(false);

            grabInputAction.performed += _grabPerformed;
            grabInputAction.canceled += _grabCanceled;

            var runInputAction = m_InputConfig.GetInputAction("Run");
            runInputAction.Enable();

            _runStarted = _ =>
            {
                _targetValue = 1;
                InputVector = moveInputAction.ReadValue<Vector2>() * _targetValue;

            };

            _runCanceled = _ =>
            {
                _targetValue = m_MoveInputScale;
                InputVector = moveInputAction.ReadValue<Vector2>() * _targetValue;
            };

            runInputAction.started += _runStarted;
            runInputAction.canceled += _runCanceled;

        }

        private void OnDisable()
        {
            var moveInputAction = m_InputConfig.GetInputAction("Move");
            moveInputAction.started -= ParseMovementInput;
            moveInputAction.performed -= ParseMovementInput;
            moveInputAction.canceled -= ParseMovementInput;

            var jumpInputAction = m_InputConfig.GetInputAction("Jump");
            jumpInputAction.performed -= _jumpPerformed;
            jumpInputAction.canceled -= _jumpCanceled;

            var grabInputAction = m_InputConfig.GetInputAction("Grab");
            grabInputAction.performed -= _grabPerformed;
            grabInputAction.canceled -= _grabCanceled;

            var runInputAction = m_InputConfig.GetInputAction("Run");
            runInputAction.started -= _runStarted;
            runInputAction.canceled -= _runCanceled;
        }

        private void Update() => ProcessMovementInput();
        public float HorizontalMoveInput { get; private set; }
        private void ProcessMovementInput()
        {
            Vector2 adjustedMovement = InputVector;

            //Accelerate/decelerate
            var targetSpeed = Mathf.Abs(adjustedMovement.x);
            float t = Time.deltaTime * m_targetSpeedReachMultiplier;
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, t);
            float currentHorizontalMoveInput = HorizontalMoveInput;
            HorizontalMoveInput = Mathf.Lerp(currentHorizontalMoveInput, adjustedMovement.normalized.x * targetSpeed, t);

            _characterController.OnMove(targetSpeed);
            _characterController.UserInput(InputVector);
            _previousSpeed = targetSpeed;
        }

        private void ParseMovementInput(InputAction.CallbackContext value)
        {
            var input = value.ReadValue<Vector2>();
            input.x *= _targetValue;
            InputVector = input;
        }
    }
}
