using System;
using UnityEngine;

namespace PigeonProject
{
    /// <summary>
    /// Marks a gameobject as a aerializable reference for saving.
    /// The ReferenceSerializer can locate this marker and register a reference.
    /// </summary>
    [ExecuteAlways]
    public class ReferenceSerializationMarker : MonoBehaviour
    {
        [SerializeField] string _guid = "";

        public GameObject Prefab { get => ReferenceSerializer.Instance.GetObject(_guid); }

        public bool IsPrefab { get => Prefab == gameObject; }


        private void Awake()
        {
            NotifyGameObject();
        }


        public string GenerateId()
        {
            if (IsPrefab)
            {
                if (_guid.Length < 1)
                {
                    _guid = Guid.NewGuid().ToString();
                    if (ReferenceSerializer.Instance != null)
                    {
                        ReferenceSerializer.Instance.InitializeMarker(this);
                        NotifyGameObject();
                    }
                }
            }
            return _guid;
        }

        public void NotifyGameObject()
        {
            BroadcastMessage("OnGuidGenerated", _guid, SendMessageOptions.DontRequireReceiver);
        }
    }
}
