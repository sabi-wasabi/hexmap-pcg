using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    [ExecuteAlways]
    [CreateAssetMenu(menuName = "PigeonProject/SaveSystem/ReferenceSerializer", fileName = "ReferenceSerializer")]
    public class ReferenceSerializer : ScriptableObject, ISerializationCallbackReceiver
    {
        public static ReferenceSerializer Instance { get => _instance; }
        static ReferenceSerializer _instance;


        private readonly IDictionary<string, GameObject> _register = new Dictionary<string, GameObject>();
        private readonly IDictionary<string, ReferenceMarkedScriptableObject> _scripoRegister = new Dictionary<string, ReferenceMarkedScriptableObject>();


        [SerializeField] private List<string> _keys = default;
        [SerializeField] private List<GameObject> _values = default;

        [SerializeField] private List<string> _scripoKeys = default;
        [SerializeField] private List<ReferenceMarkedScriptableObject> _scripoValues = default;


        #region Unity Methods
        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        public void OnAfterDeserialize()
        {
            _register.Clear();
            for (int i = 0; i < _keys.Count; i++)
                _register.Add(_keys[i], _values[i]);
            _scripoRegister.Clear();
            for (int i = 0; i < _scripoKeys.Count; i++)
                _scripoRegister.Add(_scripoKeys[i], _scripoValues[i]);
        }

        public void OnBeforeSerialize()
        {
            _keys = new List<string>(_register.Keys);
            _values = new List<GameObject>(_register.Values);
            _scripoKeys = new List<string>(_scripoRegister.Keys);
            _scripoValues = new List<ReferenceMarkedScriptableObject>(_scripoRegister.Values);
        }
        #endregion


        /// <summary>
        /// Gets the guid of a gameobject.
        /// If no guid was generated yet, a new guid is generated.
        /// </summary>
        /// <remarks>
        /// After creating new references the ReferenceSerializer needs to rebuild its register.
        /// </remarks>
        /// <param name="gameobject">The gameobject to be referenced.</param>
        /// <returns>the guid if a marker was found, empty string otherwise.</returns>
        public string SetReference(GameObject gameobject)
        {
            if (gameobject != null && gameobject.TryGetComponent(out ReferenceSerializationMarker marker))
                return SetReference(marker);
            Debug.LogWarning($"{gameobject.name} does not have a {nameof(ReferenceSerializationMarker)} component. This is needed for generating the guid.");
            return "";
        }
        /// <summary>
        /// Gets the guid of a gameobject.
        /// If no guid was generated yet, a new guid is generated.
        /// </summary>
        /// <remarks>
        /// After creating new references the ReferenceSerializer needs to rebuild its register.
        /// </remarks>
        /// <param name="marker">The ReferenceSerializationMarker to be referenced.</param>
        /// <returns>the guid of the reference.</returns>
        public string SetReference(ReferenceSerializationMarker marker)
        {
            return marker != null ? marker.GenerateId() : null;
        }
        /// <summary>
        /// Gets the guid of a referencable scriptable object.
        /// </summary>
        /// <param name="scripo">The scriptable object to be referenced. Must inherit from ReferenceMakredScriptableObject.</param>
        /// <returns>the guid of the reference.</returns>
        public string SetScripoReference(ReferenceMarkedScriptableObject scripo)
        {
            return scripo != null ? scripo.GenerateId() : null;
        }

        /// <summary>
        /// Gets a gameobject instance by guid.
        /// </summary>
        /// <param name="key">The guid of the instance.</param>
        /// <returns>the gameobject if it was registered; null otherwise.</returns>
        public GameObject GetObject(string key)
        {
            if (key != null && _register.ContainsKey(key))
                return _register[key];
            return null;
        }
        /// <summary>
        /// Gets a scriptable object instance by guid.
        /// </summary>
        /// <param name="key">The guid of the instance.</param>
        /// <returns>the scriptable object if it was registered; null otherwise.</returns>
        public ScriptableObject GetScriptableObject(string key)
        {
            if (key != null && _scripoRegister.ContainsKey(key))
                return _scripoRegister[key];
            return null;
        }

        /// <summary>
        /// Locates all ReferenceSerializationMarker and ReferenceMarkedScriptableObjects in the project and registers them.
        /// </summary>
        public void BuildReferences()
        {
#if UNITY_EDITOR
            var markers = Resources.FindObjectsOfTypeAll<ReferenceSerializationMarker>();
            _register.Clear();
            foreach (var obj in markers)
            {
                var guid = obj.GenerateId();
                if (!obj.gameObject.scene.isLoaded)         //RM do not register references for gameobjects in a scene (we basically only want to reference prefabs)
                    _register.Add(guid, obj.gameObject);
            }
            var scripoMarkers = Resources.FindObjectsOfTypeAll<ReferenceMarkedScriptableObject>();
            _scripoRegister.Clear();
            foreach (var obj in scripoMarkers)
            {
                var guid = obj.GenerateId();
                _scripoRegister.Add(guid, obj);
            }
            OnBeforeSerialize();
#endif
        }

        /// <summary>
        /// Registers a new marker.
        /// </summary>
        /// <remarks>
        /// This is called by the marker when intitalizing the guid.
        /// You do not need to call this method manually.
        /// </remarks>
        /// <param name="marker">The marker to be registered.</param>
        public void InitializeMarker(ReferenceSerializationMarker marker)
        {
            var guid = marker.GenerateId();
            if (!_register.ContainsKey(guid))
                _register.Add(guid, marker.gameObject);
            OnBeforeSerialize();
        }
        /// <summary>
        /// Regsiters a new marker from a scriptable object.
        /// </summary>
        /// <remarks>
        /// This is called by the marker when intitalizing the guid.
        /// You do not need to call this method manually.
        /// </remarks>
        /// <param name="scripoMarker">The marker to be registered.</param>
        public void InitializeScripoMarker(ReferenceMarkedScriptableObject scripoMarker)
        {
            var guid = scripoMarker.GenerateId();
            if (!_scripoRegister.ContainsKey(guid))
                _scripoRegister.Add(guid, scripoMarker);
            OnBeforeSerialize();
        }
    }
}
