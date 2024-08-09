using SAS.Utilities;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller 2D/Downward Movement Config")]
    public class DownwardMovementConfig : ScriptableObject
    {
        public float gravityMultiplier = 5;
        public floatRange fallSpeedRange  = new floatRange(-50, 100);
    }
}
