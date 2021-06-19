// The Pigeon Protocol

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PigeonProject
{
    public delegate void OnEventRaisedEffect(object param);

    [CreateAssetMenu(menuName = "PigeonProject/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField] UnityEvent<object> _scripoListeners = new UnityEvent<object>();
        
        #region Properties
        private readonly List<GameEventListener> _listeners = new List<GameEventListener>();
        private OnEventRaisedEffect _eventRaisedCallbacks = default;

        private readonly Queue<object> _parameterQueue = new Queue<object>();
        #endregion

        #region Notes
// #if UNITY_EDITOR
#pragma warning disable CS0414
        [Header("Notes")]
        [SerializeField] private string _parameterType = default;    //RM hacky implementation of a notes field
        [SerializeField] [TextArea(2, 20)] private string _additionalNotes = default;
#pragma warning restore CS0414
// #endif
#endregion


        #region Raise
        public void Raise()
        {
            if (_parameterQueue.Count < 1)      //RM needed for backwards compatibility with empty event calls
                _parameterQueue.Enqueue(null);

            while (_parameterQueue.Count > 0)
            {
                object parameter = _parameterQueue.Dequeue();

                // Iterate MonoBehaviour listeners
                for (int i=_listeners.Count - 1; i>=0; i--)
                {
                    _listeners[i].GetResponses(this).Invoke(parameter);
                }

                // Iterate Scripo listeners
                _scripoListeners.Invoke(parameter);

                // Invoke delegate listeners
                _eventRaisedCallbacks?.Invoke(parameter);
            }

        }
        public void Raise(object param)
        {
            _parameterQueue.Enqueue(param);

            // Start event invokation
            if (_parameterQueue.Count <= 1)
                Raise();
        }
        #endregion

        #region Registration
        public void RegisterListener(GameEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
            else
                Debug.LogWarning("Trying to register listener that is already registered to this event!");
        }

        public void RemoveListener(GameEventListener listener)
        {
            if (!_listeners.Remove(listener))
                Debug.LogWarning("Trying to unregister listener that is not registered to this event!");
        }

        /// <summary>
        /// Register responses to the event from non-monobehaviour scripts.
        /// </summary>
        public void AddCallback(OnEventRaisedEffect function)
        {
            _eventRaisedCallbacks += function;
        }

        /// <summary>
        /// Unregister responses to the event from non-monobehaviour scripts.
        /// </summary>
        public void RemoveCallback(OnEventRaisedEffect function)
        {
            _eventRaisedCallbacks -= function;
        }
        #endregion
    }
}
