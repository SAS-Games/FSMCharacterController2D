using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class BoxCast : BaseCast
    {
        [SerializeField] private Vector2 m_Size;
        public override bool IsDetected()
        {
            return Physics2D.OverlapBox(m_Origin.position, m_Size, m_Layer);
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawCube(m_Origin.position, m_Size);
        }
    }
}
