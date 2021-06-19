using UnityEngine;

namespace PigeonProject
{
    public abstract class BaseCameraBehavior : ScriptableObject, ICameraBehavior
    {
        #region Settings
        [Header("Settings")]
        [SerializeField] protected Vector3 _position = default;
        [SerializeField] protected Vector3 _rotation = default;
        [Range(0, 179)]
        [SerializeField] protected float _fov = default;
        #endregion

        #region Properties
        public Vector3 InitPosition
        {
            get => _position;
            set => _position = value;
        }

        public Vector3 InitRotation
        {
            get => _rotation;
            set => _rotation = value;
        }
        public float InitFoV
        {
            get => _fov;
            set => _fov = value;
        }
        #endregion

        public abstract Vector3 GetOffset();
        public abstract Quaternion GetRotation();
        public abstract float GetFoV();
    }
}
