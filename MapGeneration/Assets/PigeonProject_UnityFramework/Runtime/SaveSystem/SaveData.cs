using System;
using System.Collections.Generic;
using UnityEngine;


namespace PigeonProject
{
    [Serializable]
    public class SaveData : ISerializationCallbackReceiver
    {
        private readonly IDictionary<string, object> _register = new Dictionary<string, object>();

        [SerializeField]
        private List<string> _keys = default;
        [SerializeReference]
        private List<object> _values = default;

        public void OnAfterDeserialize()
        {
            _register.Clear();
            for (int i=0; i<_keys.Count; i++)
            {
                _register.Add(_keys[i], _values[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            _keys = new List<string>(_register.Keys);
            _values = new List<object>(_register.Values);
        }


        /// <summary>
        /// Registers a dataset to be saved to disk.
        /// This should be called in a BeforeSave event callback.
        /// </summary>
        /// <param name="key">The key under which the dataset will be stored.</param>
        /// <param name="data">The dataset to be stored. Needs to be serializable.</param>
        public void SetData(string key, object data)
        {
            if (!_register.ContainsKey(key))
                _register.Add(key, data);
            else
                _register[key] = data;
        }

        /// <summary>
        /// Retrieve a dataset after loading from disk.
        /// This should be called in an AfterLoad event callback.
        /// </summary>
        /// <param name="key">The key under which the dataset was saved.</param>
        /// <returns>The dataset if the key was present, null otherwise.</returns>
        public object GetData(string key)
        {
            if (_register.ContainsKey(key))
                return _register[key];
            return null;
        }
        /// <summary>
        /// Retrieve a dataset after loading from disk and cast it to target type.
        /// This should be called in an AfterLoad event callback.
        /// </summary>
        /// <param name="T">The target type in which the dataset will be cast.</param>
        /// <param name="T">The target type in which the dataset will be cast.</param>
        /// <returns>The dataset as target type if the key was present, null otherwise.</returns>
        public T GetData<T>(string key)
        {
            var data = GetData(key);
            if (data is T dataAsType)
                return dataAsType;
            return default;
        }
        /// <summary>
        /// Try to retrieve a dataset after loading from disk.
        /// This should be called in an AfterLoad event callback.
        /// </summary>
        /// <param name="T">The target type in which the dataset will be cast.</param>
        /// <param name="T">The target type in which the dataset will be cast.</param>
        /// <param name="data">The output parameter for the dataset.</param>
        /// <returns>true if the key was present and the cast was successful; false otherwise.</returns>
        public bool TryGetData<T>(string key, out T data)
        {
            data = GetData<T>(key);
            return !EqualityComparer<T>.Default.Equals(data, default);
        }

        /// <summary>
        /// Remove a dataset from the local register.
        /// </summary>
        /// <remarks>
        /// This does not necessarily remove the dataset from the save file on disk.
        /// </remarks>
        /// <param name="key">The key of the dataset to be removed</param>
        /// <returns>true if the key was present; false otherwise.</returns>
        public bool RemoveData(string key)
        {
            return _register.Remove(key);
        }

        /// <summary>
        /// Resets the local register.
        /// </summary>
        /// <remarks>
        /// This does not necessarily reset the save file on disk.
        /// </remarks>
        public void ResetData()
        {
            _register.Clear();
        }
    }
}
