using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller 2D/Jump Data")]
    public class JumpData : ScriptableObject
    {
        [field: SerializeField] public int AmountOfJumps { get; private set; } = 1;
        [field: SerializeField] public float CoyoteTime { get; private set; } = 0.2f;
    }
}
