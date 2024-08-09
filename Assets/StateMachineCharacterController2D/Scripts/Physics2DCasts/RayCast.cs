using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class RayCast : BaseCast
    {
        [SerializeField] private float m_Distance;
        [SerializeField] private Vector2 m_Direction;

        public override bool IsDetected()
        {
            return Physics2D.Raycast(m_Origin.position, m_Direction, m_Distance, m_Layer);
        }

        public bool IsDetected(Vector2 dir)
        {
            m_Direction = dir;
            return IsDetected();
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawLine(m_Origin.position, m_Origin.position + (Vector3)m_Direction * m_Distance);
        }
    }
}
