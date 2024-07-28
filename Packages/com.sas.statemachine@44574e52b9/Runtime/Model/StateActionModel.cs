﻿using UnityEngine;
using System;
using System.Collections.Generic;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineGraph
{
    [Serializable]
    internal class StateActionModel : SerializedType
    {
        internal string Name => Sanitize(ToType().ToString());
        [SerializeField] internal Tag tag = default;
        [SerializeField] internal string key = default;
        [SerializeField] internal ActionExecuteEvent whenToExecute = default;
        public override bool Equals(object obj)
        {
            return Name.Equals((obj as StateActionModel).Name);
        }

        public override int GetHashCode()
        {
            return (Name + tag.ToString() + key).GetHashCode();
        }

        internal IStateAction[] GetActions(StateMachine stateMachine, Dictionary<StateActionModel, object[]> createdInstances)
        {
            IStateAction[] stateActions;
            if (createdInstances.TryGetValue(this, out var actions))
                return actions as IStateAction[];
            Type type = ToType();
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                actions = stateMachine.Actor.GetComponentsInChildren(type, tag, true);
                if (actions?.Length > 0)
                {
                    stateActions = new IStateAction[actions.Length];
                    for (int i = 0; i < actions.Length; i++)
                        stateActions[i] = actions[i] as IStateAction;
                }
                else
                {
                    Debug.LogError($"Mono Action {type} with tag {tag}  is not found!  Try attaching it under Actor {stateMachine.Actor}");
                    return null;
                }
            }
            else
                stateActions = CreateStateActionInstance(stateMachine);

            createdInstances.Add(this, stateActions);
            foreach (var action in stateActions)
                action?.OnInitialize(stateMachine.Actor, tag, key);

            return stateActions;
        }

        private IStateAction[] CreateStateActionInstance(StateMachine stateMachine)
        {
            StateActionPair stateActionPair = stateMachine.stateActionPairs.Find(ele => ele.original.Equals(fullName));
            if(stateActionPair != null)
            {
                if (!string.IsNullOrEmpty(stateActionPair.overridden))
                    return new IStateAction[] { Activator.CreateInstance(Type.GetType(stateActionPair.overridden)) as IStateAction };
            }

            return new IStateAction[] { Activator.CreateInstance(ToType()) as IStateAction };
        }
    }
}
