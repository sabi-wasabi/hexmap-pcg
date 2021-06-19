using UnityEngine;
using UnityEngine.Events;

namespace PigeonProject
{
    public class DebugMethodCaller : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] public bool[]  ComponentToggles = new bool[0];
    }
}
