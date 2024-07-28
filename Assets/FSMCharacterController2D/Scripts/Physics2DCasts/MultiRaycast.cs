using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class MultiRaycast : BaseCast
    {
        [SerializeField] private int m_RayCount = 2;
        [SerializeField] private float m_Distance;
        [SerializeField] private float m_Width;

        private float rot;

        public bool IsDetected(float rotation)
        {
            rot = rotation * Mathf.Deg2Rad;
            var up = new Vector2(-Mathf.Sin(rot), Mathf.Cos(rot));
            var right = new Vector2(up.y, -up.x);

            Vector2 rayPoint = m_Origin.position;
            // Is the middle ray good?
            var isGroundedThisFrame = PerformRay(rayPoint);

            // If not, zigzag rays from the center outward until we find a hit
            if (!isGroundedThisFrame)
            {
                foreach (var offset in GenerateRayOffsets())
                {
                    isGroundedThisFrame = PerformRay(rayPoint + right * offset) || PerformRay(rayPoint - right * offset);
                    if (isGroundedThisFrame) return true;
                }
                return false;
            }
            return true;

            bool PerformRay(Vector2 point)
            {
                var groundHit = Physics2D.Raycast(point, -up, m_Distance, m_Layer);
                if (!groundHit) 
                    return false;

                if (Vector2.Angle(groundHit.normal, up) > 60)
                    return false;

                return true;
            }
        }

        public override bool IsDetected()
        {
            return false;
        }

        protected override void DrawGizmos()
        {
            var up = new Vector2(-Mathf.Sin(rot), Mathf.Cos(rot));
            var right = new Vector2(up.y, -up.x);

            Gizmos.color = m_GizmoColor;
            Gizmos.DrawLine(m_Origin.position, m_Origin.position + (Vector3)(-up * m_Distance));

            foreach (var offset in GenerateRayOffsets())
            {
                Gizmos.DrawLine(m_Origin.position + (Vector3)(right * offset), m_Origin.position + (Vector3)(right * offset) + (Vector3)(-up * m_Distance));
                Gizmos.DrawLine(m_Origin.position - (Vector3)(right * offset), m_Origin.position - (Vector3)(right * offset) + (Vector3)(-up * m_Distance));
            }
        }

        private IEnumerable<float> GenerateRayOffsets()
        {
            var offsetAmount = m_Width / m_RayCount;
            for (var i = 1; i < m_RayCount + 1; i++)
            {
                yield return offsetAmount * i;
            }
        }
    }
}
