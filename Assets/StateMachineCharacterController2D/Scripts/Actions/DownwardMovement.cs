using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController2D
{
	public class DownwardMovement : IStateAction
	{
		private FSMCharacterController2D _characterController;
		private DownwardMovementConfig _downwardMovementConfig = default;
		private float _verticalMovement;

		void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
		{
			actor.TryGet(out _downwardMovementConfig, key);
			actor.TryGetComponent(out _characterController);
		}

		void IStateAction.Execute(ActionExecuteEvent executeEvent)
		{
			if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
				_verticalMovement = _characterController.CurrentVelocity.y;
				return;
            }
			_verticalMovement += Physics2D.gravity.y * _downwardMovementConfig.gravityMultiplier * Time.deltaTime;
			_verticalMovement = Mathf.Clamp(_verticalMovement, _downwardMovementConfig.fallSpeedRange.min, _downwardMovementConfig.fallSpeedRange.max);

			_characterController.SetVelocityY(_verticalMovement);
		}
	}
}
