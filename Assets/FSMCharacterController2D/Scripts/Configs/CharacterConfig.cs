using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public struct CharacterSize
    {
        // Standing
        public float Height;
        public float Width;
        public float StepOffset;
        public float RayInset;
        public Vector2 StandingColliderSize;
        public Vector2 StandingColliderCenter;

        // Crouching
        public Vector2 CrouchColliderSize;
        public float CrouchingHeight;
        public Vector2 CrouchingColliderCenter;
    }

    [CreateAssetMenu]
    public class CharacterConfig : ScriptableObject
    {
        public LayerMask CollisionLayers;
        public float SlopeLimit = 50;
        public const float STEP_BUFFER = 0.05f;
        public const float COLLIDER_EDGE_RADIUS = 0.05f;

        [Range(0.1f, 10), Tooltip("How tall you are. This includes a collider and your step height.")]
        public float Height = 1.8f;

        [Range(0.1f, 10), Tooltip("The width of your collider")]
        public float Width = 0.6f;

        [Range(STEP_BUFFER, 15), Tooltip("Step height allows you to step over rough terrain like steps and rocks.")]
        public float StepHeight = 0.5f;

        [Range(0.1f, 10), Tooltip("A percentage of your height stat which determines your height while crouching. A smaller crouch requires more step height sacrifice")]
        public float CrouchHeight = 0.6f;

        [Range(0.01f, 0.2f), Tooltip("The outer buffer distance of the grounder rays. Reducing this too much can cause problems on slopes, too big and you can get stuck on the sides of drops.")]
        public float RayInset = 0.1f;

        public CharacterSize GenerateCharacterSize()
        {
            ValidateHeights();

            var s = new CharacterSize
            {
                Height = Height,
                Width = Width,
                StepOffset = StepHeight,
                RayInset = RayInset
            };

            s.StandingColliderSize = new Vector2(s.Width - COLLIDER_EDGE_RADIUS * 2, s.Height - COLLIDER_EDGE_RADIUS * 2);
            s.StandingColliderCenter = new Vector2(0, s.Height - s.StandingColliderSize.y / 2 - COLLIDER_EDGE_RADIUS);

            s.CrouchingHeight = CrouchHeight;
            s.CrouchColliderSize = new Vector2(s.Width - COLLIDER_EDGE_RADIUS * 2, s.CrouchingHeight);
            s.CrouchingColliderCenter = new Vector2(0, s.CrouchingHeight - s.CrouchColliderSize.y / 2 - COLLIDER_EDGE_RADIUS);

            return s;
        }

        private static double _lastDebugLogTime;
        private const double TIME_BETWEEN_LOGS = 1f;

        private void ValidateHeights()
        {
#if UNITY_EDITOR
            var maxStepHeight = Height - STEP_BUFFER;
            if (StepHeight > maxStepHeight)
            {
                StepHeight = maxStepHeight;
                Log("Step height cannot be larger than height");
            }

            var minCrouchHeight = StepHeight + STEP_BUFFER;

            if (CrouchHeight < minCrouchHeight)
            {
                CrouchHeight = minCrouchHeight;
                Log("Crouch height must be larger than step height");
            }

            void Log(string text)
            {
                var time = EditorApplication.timeSinceStartup;
                if (_lastDebugLogTime + TIME_BETWEEN_LOGS > time) return;
                _lastDebugLogTime = time;
                Debug.LogWarning(text);
            }
#endif
        }
    }

    [Serializable]
    public enum PositionCorrectionMode
    {
        Velocity,
        Immediate
    }
}