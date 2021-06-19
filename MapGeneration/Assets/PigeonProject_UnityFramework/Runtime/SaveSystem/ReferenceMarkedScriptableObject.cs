using System;
using UnityEngine;

namespace PigeonProject
{
    public class ReferenceMarkedScriptableObject : ScriptableObject
    {
        [SerializeField] string _guid = "";

        public string GenerateId()
        {
            if (_guid.Length < 1)
            {
                _guid = Guid.NewGuid().ToString();
                if (ReferenceSerializer.Instance != null)
                {
                    ReferenceSerializer.Instance.InitializeScripoMarker(this);
                }
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
            return _guid;
        }
    }
}
