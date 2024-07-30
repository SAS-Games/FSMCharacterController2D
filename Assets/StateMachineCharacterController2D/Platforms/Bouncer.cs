using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public class Bouncer : MonoBehaviour
    {
        [SerializeField] private float _bounceForce = 60f;
        [SerializeField] private float _maxForce = 20;
        [SerializeField] private bool _useSurfaceNormal;
        [SerializeField] private AudioClip _clip;

        void ApplyBounceForce(Collider2D collision)
        {
            var controller = collision.gameObject.GetComponentInParent<FSMCharacterController2D>();
            var pos = transform.position;
            Vector2 force;

            if (_useSurfaceNormal)
            {
                var collisionPoint = collision.ClosestPoint(pos);
                var collisionNormal = pos - (Vector3)collisionPoint;
                force = -collisionNormal;
            }
            else
            {
                var incomingSpeedNormal = Vector3.Project(controller.CurrentVelocity, transform.up);
                force = -incomingSpeedNormal;
            }

            force = Vector2.ClampMagnitude(force * _bounceForce, _maxForce);
            controller.AddFrameForce(force, true);
            controller.Actor.SetState("Bounce");

            AudioSource.PlayClipAtPoint(_clip, pos, Mathf.InverseLerp(0, _maxForce, force.magnitude));
        }
    }
}