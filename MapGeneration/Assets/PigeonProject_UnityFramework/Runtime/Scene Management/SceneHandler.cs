using UnityEngine;

namespace PigeonProject
{
    public abstract class SceneHandler : MonoBehaviour
    {
        public virtual void Initialize() { }
        public virtual void Terminate() { }
    }
}
