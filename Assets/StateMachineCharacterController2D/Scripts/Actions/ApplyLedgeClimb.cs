//using SAS.StateMachineGraph;
//using SAS.Utilities.TagSystem;
//using UnityEngine;

//namespace SAS.StateMachineCharacterController2D
//{
//    public class ApplyLedgeClimb : IStateAction
//    {
//        [FieldRequiresSelf] private FSMCharacterController2D _characterController;
//        private Vector2 cornerPos;
//        private Vector2 startPos;
//        private Vector2 workspace;

//        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
//        {
//            actor.Initialize(this);
//        }

//        void IStateAction.Execute(ActionExecuteEvent executeEvent)
//        {
//            if (executeEvent == ActionExecuteEvent.OnStateEnter)
//            {
//                _characterController.SetVelocityZero();
//                cornerPos = DetermineCornerPosition();

//                startPos.Set(cornerPos.x - (_characterController.FacingDirection * _characterController.playerData.startOffset.x), cornerPos.y - _characterController.playerData.startOffset.y);
//                _characterController.edgeClimbedStopPosition.Set(cornerPos.x + (_characterController.FacingDirection * _characterController.playerData.stopOffset.x), cornerPos.y + _characterController.playerData.stopOffset.y);

//                _characterController.transform.position = startPos;
//            }

//            _characterController.SetVelocityZero();
//            _characterController.transform.position = startPos;
//        }

//        private Vector2 DetermineCornerPosition()
//        {
//            RaycastHit2D xHit = Physics2D.Raycast(_characterController.WallCheck.position, Vector2.right * _characterController.FacingDirection, _characterController.WallCheckDistance, _characterController.WhatIsGround);
//            float xDist = xHit.distance;
//            workspace.Set((xDist + 0.015f) * _characterController.FacingDirection, 0f);
//            RaycastHit2D yHit = Physics2D.Raycast(_characterController.LedgeCheckHorizontal.position + (Vector3)(workspace), Vector2.down, _characterController.LedgeCheckHorizontal.position.y - _characterController.WallCheck.position.y + 0.015f, _characterController.WhatIsGround);
//            float yDist = yHit.distance;

//            workspace.Set(_characterController.WallCheck.position.x + (xDist * _characterController.FacingDirection), _characterController.LedgeCheckHorizontal.position.y - yDist);
//            return workspace;
//        }
//    }
//}