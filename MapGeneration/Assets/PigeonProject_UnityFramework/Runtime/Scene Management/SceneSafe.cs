using System.Collections.Generic;
using PigeonProject;

namespace PigeonProject
{
    public static class SceneSafe
    {
        private static readonly IDictionary<string, object> _safe = new Dictionary<string, object>();

        public static void SaveObject(string key, object savedObject)
        {
            if (_safe.ContainsKey(key))
                _safe[key] = savedObject;
            else
                _safe.Add(key, savedObject);
        }

        public static object GetSavedObject(string key)
        {
            if (!_safe.ContainsKey(key))
            {
                Perry.LogWarning($"{key} does not exist in the Scene Safe!");
                return null;
            }

            return _safe[key];
        }
        public static T GetSavedObject<T>(string key)
        {
            object savedObject = GetSavedObject(key);

            if (savedObject == null) return default;

            try
            {
                return (T)savedObject;
            }
            catch
            {
                Perry.LogWarning($"{key} is not of Type {typeof(T)}!");
                return default;
            }
        }

        public static bool TryGetSavedObject(string key, out object output)
        {
            output = GetSavedObject(key);
            if (output == null)
                return false;
            else
                return true;
        }
        public static bool TryGetSavedObject<T>(string key, out T output)
        {
            output = GetSavedObject<T>(key);
            if (output.Equals(default))
                return false;
            else
                return true;
        }
    }
}
