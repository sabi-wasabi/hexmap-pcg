// The Pigeon Protocol

using UnityEngine;
using System;

namespace PigeonProject
{
    public abstract class BaseReferenceVariable<T> : GameEvent
    {
        #region Properties
        public T InitialValue { get => _initialValue; }
        [SerializeField] [Tooltip("Serialized Value - RuntimeValue gets set to this at the start.")]
        protected T _initialValue = default;

        public T RuntimeValue
        {
            get => _runtimeValue;
            set
            {
                if (!_isReadonly)
                {
                    bool invokeEvent = false;
                    if (_raiseGameEvent && !_runtimeValue.Equals(value))
                        invokeEvent = true;
                    _runtimeValue = value;
                    if (invokeEvent)
                        Raise(this);
                }
                else
                {
                    Debug.LogError("Cannot assign to a Reference Variable that is declared readonly!");
                }
            }
        }
        [SerializeField] [Tooltip("Current value during Runtime.")]
        protected T _runtimeValue;

        [SerializeField] [Tooltip("Whether it is possible to assign this value from other scripts.")]
        private bool _isReadonly = false;
        #endregion

        #region Member Variables
        [SerializeField]
        private bool _raiseGameEvent = false;
        #endregion

        #region Notes
        // #if UNITY_EDITOR
#pragma warning disable CS0414
        [Header("Notes")]
        [SerializeField] [TextArea(2, 20)] private string _variableNotes = default;
#pragma warning restore CS0414
// #endif
        #endregion


        #region Unity methods
        private void OnEnable()
        {
            _runtimeValue = _initialValue;
            if (_raiseGameEvent)
                Raise(this);
        }
        #endregion

        #region Public methods
        public void UpdateInitialValue()
        {
            _initialValue = _runtimeValue;
        }

        public void ResetRuntimeValue()
        {
            RuntimeValue = _initialValue;
        }
        #endregion

        #region Private methods

        #endregion
    }
}
