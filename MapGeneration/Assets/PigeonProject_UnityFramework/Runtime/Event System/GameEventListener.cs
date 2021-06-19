// The Pigeon Protocol

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PigeonProject
{
    [ExecuteAlways]
    public class GameEventListener : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private List<GameEvent> _gameEvents = new List<GameEvent>();

        [SerializeField]
        private List<UnityObjectEvent> _responses = new List<UnityObjectEvent>();
        #endregion

        #region Member Variables
        [SerializeField]
        [HideInInspector]
        private List<bool> _editorFoldout = new List<bool>();
        #endregion


        #region Unity methods
        private void Awake()
        {
            foreach (var gameEvent in _gameEvents)
            {
                if (gameEvent != null)
                    gameEvent.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            foreach (var gameEvent in _gameEvents)
            {
                if (gameEvent != null)
                    gameEvent.RemoveListener(this);
            }
        }
        #endregion


        #region Public methods
        public UnityObjectEvent GetResponses(GameEvent gameEvent)
        {
            int index = _gameEvents.IndexOf(gameEvent);
            if (index < 0)
                return null;
            return _responses[index];
        }

        public void AddEvent()
        {
            _gameEvents.Add(null);
            _responses.Add(new UnityObjectEvent());
            _editorFoldout.Add(true);
        }

        public void RemoveEvent(int index)
        {
            _gameEvents.RemoveAt(index);
            _responses.RemoveAt(index);
            _editorFoldout.RemoveAt(index);
        }
        #endregion

    }

    [Serializable]
    public class UnityObjectEvent : UnityEvent<object> { }
}
