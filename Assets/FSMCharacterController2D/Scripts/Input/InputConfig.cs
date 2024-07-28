using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController2D
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller/Input")]
    public class InputConfig : ScriptableObject
    {
        [System.Serializable]
        class Input
        {
            [SerializeField] private string m_Key;
            [SerializeField] private InputActionReference m_InputActionReference;

            public string Key => m_Key;
            public InputActionReference Value => m_InputActionReference;
        }

        [SerializeField] private Input[] m_Inputs;
        [NonSerialized] private Dictionary<string, InputActionReference> _inputs = new Dictionary<string, InputActionReference>();
        [NonSerialized] private bool _initialized = false;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized)
                return;

            _inputs.Clear();
            _initialized = true;

            foreach (var input in m_Inputs)
                _inputs.Add(input.Key, input.Value);
        }

        public InputAction GetInputAction(string key)
        {
            return Get(key);
        }

        public InputActionReference Get(string key)
        {
            Initialize();

            if (TryGet(key, out var value))
                return value;
            return null;
        }

        private bool TryGet(string key, out InputActionReference value)
        {
            if (!_inputs.TryGetValue(key, out value))
            {
                value = null;
                return false;
            }
            return true;
        }
    }
}
