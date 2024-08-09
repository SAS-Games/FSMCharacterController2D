using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
	[CreateAssetMenu(fileName = "Wall Jump Config", menuName = "SAS/State Machine Character Controller 2D/Wall Jump Config")]
	public class WallJumpConfig : ScriptableObject
	{
        [field: SerializeField] public float Velocity { get; private set; } = 20;
        [field: SerializeField] public Vector2 Angle { get; private set; } = new Vector2(1, 2);
    }
}