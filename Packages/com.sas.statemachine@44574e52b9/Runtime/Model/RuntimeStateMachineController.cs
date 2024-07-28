﻿using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineGraph
{
    public class RuntimeStateMachineController : ScriptableObject
    {
        [SerializeField, HideInInspector] private StateMachineModel m_BaseStateMachineModel = default;
        [SerializeField, HideInInspector] private StateMachineParameter[] _parameters = default;
        [SerializeField, HideInInspector] private StateModel m_DefaultStateModel = default;
        [SerializeField, HideInInspector] private StateModel m_AnyStateModel = default;

        internal int GetOriginalClipsCount { get; }

        private void Awake()
        {
#if UNITY_EDITOR
           var controller =  (this is StateMachineOverrideController) ? (this as StateMachineOverrideController).runtimeStateMachineController : this;
            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            var fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            name = fileName;
#endif
        }

        internal StateMachine CreateStateMachine(Actor actor, StateMachineOverrideController stateMachineOverrideController)
        {
            List<StateActionPair> stateActionPairs = new List<StateActionPair>();
            if (stateMachineOverrideController != null)
            {
                stateActionPairs = stateMachineOverrideController.stateActionPairs;
            }
            StateMachine stateMachine = new StateMachine(actor, _parameters, stateActionPairs);
            var cachedState = new Dictionary<ScriptableObject, object>();
            var cachedActions = new Dictionary<StateActionModel, object[]>();
            var cachedTriggers = new Dictionary<string, ICustomCondition>();

            var stateModels = m_BaseStateMachineModel.GetStatesRecursivily();
            stateModels.Add(m_AnyStateModel);

            List<State> states = new();   
            foreach (StateModel stateModel in stateModels)
            {
                var state = stateModel.GetState(stateMachine, cachedState, cachedActions, cachedTriggers);
                states.Add(state);
                if (stateModel == m_DefaultStateModel)
                    stateMachine.DefaultState = state;
                else if (stateModel == m_AnyStateModel)
                    stateMachine.AnyState = state;
            }
            stateMachine.states.AddRange(states);
            return stateMachine;
        }

        internal void Initialize(RuntimeStateMachineController runtimeStateMachineController)
        {
            name = runtimeStateMachineController.name;
            _parameters = new StateMachineParameter[runtimeStateMachineController._parameters.Length];

            for (int i = 0; i < _parameters.Length; ++i)
                _parameters[i] = new StateMachineParameter(runtimeStateMachineController._parameters[i]);

            m_BaseStateMachineModel = runtimeStateMachineController.m_BaseStateMachineModel;
            m_DefaultStateModel = runtimeStateMachineController.m_DefaultStateModel;
            m_AnyStateModel = runtimeStateMachineController.m_AnyStateModel;
        }
    }
}
