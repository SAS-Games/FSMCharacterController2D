using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class CircleCast : BaseCast
    {
        [SerializeField] private float m_Radius;
        public override bool IsDetected()
        {
            return Physics2D.OverlapCircle(m_Origin.position, m_Radius, m_Layer);
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawWireSphere(m_Origin.position, m_Radius);
        }
    }
}
