using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
	[CreateAssetMenu(fileName = "AerialMovement", menuName = "SAS/State Machine Character Controller 2D/Aerial Movement Config")]
	public class AerialMovementConfig : ScriptableObject
	{
		public float speed = 7;
		public float acceleration = 30;
		public float airResistance = 5f;
	}
}