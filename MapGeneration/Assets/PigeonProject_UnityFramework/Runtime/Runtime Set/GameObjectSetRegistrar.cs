// The Pigeon Protocol

using UnityEngine;

namespace PigeonProject
{
    public class GameObjectSetRegistrar : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private GameObjectSet gameObjectSet = default;
        public GameObjectSet GameObjectSet { get => gameObjectSet; }
        #endregion


        #region Public methods

        #endregion

        #region Private methods
        private void Awake()
        {
            gameObjectSet?.Add(gameObject);
        }

        private void OnDestroy()
        {
            gameObjectSet?.Remove(gameObject);
        }
        #endregion
    }
}
