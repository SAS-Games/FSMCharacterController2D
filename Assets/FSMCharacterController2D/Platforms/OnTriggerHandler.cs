using System.Linq;
using UnityEngine;

public class OnTriggerHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_MessageListener;
    [SerializeField] private string m_Message;
    [SerializeField] private string[] m_CollisionTags = { "Player" };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_CollisionTags.Contains(collision.tag))
            m_MessageListener?.SendMessage(m_Message, collision, SendMessageOptions.DontRequireReceiver);
    }
}
