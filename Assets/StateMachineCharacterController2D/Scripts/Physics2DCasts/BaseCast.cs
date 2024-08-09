using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public abstract class BaseCast : MonoBehaviour
    {
        [SerializeField] protected Transform m_Origin;        
        [SerializeField] protected LayerMask m_Layer;
        [SerializeField] protected Color m_GizmoColor = Color.red; 


        public abstract bool IsDetected();
        protected abstract void DrawGizmos();

        private void OnDrawGizmos()
        {
            if (m_Origin != null)
            {
                Gizmos.color = m_GizmoColor;
                DrawGizmos();
            }
        }
    }
}
