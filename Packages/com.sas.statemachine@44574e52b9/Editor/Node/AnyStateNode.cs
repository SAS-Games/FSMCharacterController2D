﻿using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SAS.StateMachineGraph.Editor
{
    public class AnyStateNode : BaseNode
    {
        private Action<BaseNode> _startTransition;
        private StateMachineModel _stateMachineModel;
        internal StateModel Value => TargetObject as StateModel;
        public AnyStateNode(StateMachineModel stateMachineModel, Object targetObject, Vector2Int position, Action<BaseNode> startTransition) :
               base(targetObject, position)
        {
            _normalStyleName = "flow node 2";
            _focusedStyleName = "flow node 2 on";
            _stateMachineModel = stateMachineModel;
            _startTransition = startTransition;
        }

        protected override void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Make Transition"), false, () => _startTransition?.Invoke(this));
            genericMenu.ShowAsContext();
        }

        protected override void ProcessMouseUp(BaseNode baseNode, Event e)
        {
        }

        public override void Drag(Vector2Int delta)
        {
            base.Drag(delta);
            _stateMachineModel.SetAnyStatePosition(Value.GetPosition()); ;
        }
    }
}
