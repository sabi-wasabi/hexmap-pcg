using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    public class TimerSystem
    {
        #region Static Properties
        public static TimerSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TimerSystem();
                return _instance;
            }
        }
        private static TimerSystem _instance = null;
        #endregion


        private readonly IDictionary<string, Timer> _idTimer = new Dictionary<string, Timer>();


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnLoad()
        {
            var baseComponents = Object.FindObjectsOfType<TimerSystemBase>();
            if (baseComponents.Length < 1)
            {
                var empty = new GameObject { name = nameof(TimerSystemBase) };
                empty.AddComponent<TimerSystemBase>().enabled = true;
            }
            else if (baseComponents.Length > 1)
            {
                for (int i = 1; i < baseComponents.Length; i++)
                    Object.Destroy(baseComponents[i]);
            }
        }

        /// <summary>
        /// Updates all registered timers and advances their state.
        /// </summary>
        /// <remarks>
        /// This gets called internally and you should not call this yourself.
        /// </remarks>
        public void Update()
        {
            foreach (var timer in _idTimer.Values)
                timer.Update();
        }


        /// <summary>
        /// Registers an existing timer instance to the system.
        /// </summary>
        /// <param name="id">A unique id under which the timer will be saved.</param>
        /// <param name="timer">The timer instance that should be registered.</param>
        /// <returns>the registered timer.</returns>
        public Timer RegisterTimer(string id, Timer timer)
        {
            if (_idTimer.ContainsKey(id))
            {
                Debug.Log($"There is already a Timer registered with id {id}!");
                return timer;
            }
            _idTimer.Add(id, timer);
            return timer;
        }
        /// <summary>
        /// Creates a new timer insatnce and registers it to the system.
        /// </summary>
        /// <param name="id">A unique id under which the timer will be saved.</param>
        /// <param name="duration">Inital duration for the timer.</param>
        /// <param name="startImmediately">Whether the timer should be set active immediately after creation.</param>
        /// <returns>the created timer.</returns>
        public Timer RegisterTimer(string id, float duration = -1f, bool startImmediately = false) => RegisterTimer(id, new Timer(duration, startImmediately));

        /// <summary>
        /// Gets a registered timer by the id it was registered with.
        /// </summary>
        /// <param name="id">Unique id under which the timer was registered.</param>
        /// <returns>The registered timer if it exists, null otherwise.</returns>
        public Timer GetTimerById(string id)
        {
            if (!_idTimer.ContainsKey(id))
            {
                Debug.Log($"Timer with id {id} is not registered!");
                return null;
            }
            return _idTimer[id];
        }

        /// <summary>
        /// Utiltiy method to check running status of an registered timer.
        /// </summary>
        /// <param name="id">Unique id under which the timer was registered.</param>
        /// <returns>true if the timer is currently running, false otherwise.</returns>
        public bool IsTimerRunning(string id)
        {
            if (!_idTimer.ContainsKey(id))
                return false;
            return _idTimer[id].IsRunning;
        }

        /// <summary>
        /// Checks whether the timer is currently running and restarts it if not.
        /// </summary>
        /// <param name="id">Unique id under which the timer was registered.</param>
        /// <returns>true if the tiemr is currently not running, false otherwise.</returns>
        public bool CheckAndStart(string id)
        {
            if (!_idTimer.ContainsKey(id))
                return false;
            if (IsTimerRunning(id))
                return false;
            _idTimer[id].Restart();
            return true;
        }


        public class TimerSystemBase : MonoBehaviour
        {
            private void OnEnable()
            {
                DontDestroyOnLoad(gameObject);
            }

            private void Update()
            {
                TimerSystem.Instance.Update();
            }
        }
    }
}
