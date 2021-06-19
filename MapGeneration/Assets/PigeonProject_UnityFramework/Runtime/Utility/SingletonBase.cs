using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PigeonProject
{
    /// <summary>
    /// Scriptable Objects wothout a reference in scene do not get called from Unity.
    /// This is a base Component placed in the scene with its only purpose to maintain references to otherwise
    /// unreferenced Scriptable Objects.
    /// </summary>
    public class SingletonBase : MonoBehaviour
    {
#pragma warning disable CS0414
        [SerializeField] private ScriptableObject[] _references = default;
#pragma warning restore CS0414

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
