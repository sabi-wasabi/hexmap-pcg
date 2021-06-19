using UnityEngine;

namespace PigeonProject
{
    public class ScripoUpdater : MonoBehaviour
    {
        [Tooltip("Must implement IUpdateReceiver")]
        [SerializeField] ScriptableObject[] _updateReceiver = default;

        [Tooltip("Must implement IFixedUpdateReceiver")]
        [SerializeField] ScriptableObject[] _fixedUpdateReceiver = default;

        void Update()
        {
            foreach (ScriptableObject receiver in _updateReceiver)
            {
                if (receiver is IUpdateReceiver updateReceiver)
                {
                    updateReceiver.Update();
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (ScriptableObject receiver in _fixedUpdateReceiver)
            {
                if (receiver is IFixedUpdateReceiver fixedUpdateReceiver)
                {
                    fixedUpdateReceiver.FixedUpdate();
                }
            }
        }
    }
}
