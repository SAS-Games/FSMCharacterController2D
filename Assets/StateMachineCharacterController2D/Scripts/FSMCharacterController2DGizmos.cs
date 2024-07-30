using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    public partial class FSMCharacterController2D
    {
        private void OnDrawGizmos()
        {
            if (!BodyCollider)
                return;
            var pos = CalculateRayPoint();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pos + Vector2.up * characterSize.Height / 2, new Vector3(characterSize.Width, characterSize.Height));
            Gizmos.color = Color.magenta;

            var rayStart = pos + Vector2.up * SKIN_WIDTH;
            var rayDir = Vector3.down * (currentStepDownLength + SKIN_WIDTH);
            Gizmos.DrawRay(rayStart, rayDir);
            foreach (var offset in GenerateRayOffsets())
            {
                Gizmos.DrawRay(rayStart + Vector2.right * offset, rayDir);
                Gizmos.DrawRay(rayStart + Vector2.left * offset, rayDir);
            }

            Gizmos.color = Color.yellow;
            Vector2 dir = Vector2.right * FacingDirection;
            float distance = BodyCollider.bounds.size.x * 0.5f + BodyCollider.edgeRadius + SKIN_WIDTH;
            Gizmos.DrawRay(pos, dir * distance);
            Gizmos.DrawRay(pos + Up * characterSize.StepOffset, dir * (distance + 0.1f));

        }

    }
}
