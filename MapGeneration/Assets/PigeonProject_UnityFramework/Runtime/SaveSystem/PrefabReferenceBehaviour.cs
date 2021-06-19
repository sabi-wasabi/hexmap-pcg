using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    public class PrefabReferenceBehaviour : MonoBehaviour
    {
        [SerializeField] ReferenceSerializationMarker _marker = default;

        public bool IsReferenceSet { get => _marker != null; }
        public GameObject Prefab { get => _marker.Prefab; }
        public bool IsPrefab { get => _marker.IsPrefab; }
        public string Guid { get => _marker.GenerateId(); }
    }
}
