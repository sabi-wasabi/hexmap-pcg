using UnityEngine;

namespace PigeonProject
{
    [CreateAssetMenu(fileName = "CameraTraumaHandler", menuName = "PigeonProject/Camera/CameraTraumaHandler")]
    public class CameraTraumaHandler : ScriptableObject, IUpdateReceiver
    {
        static public CameraTraumaHandler Instance { get; private set; }

        #region Settings
        [Header("Reference Variables")]
        [SerializeField] private FloatVariable _traumaVariable = default;
        [Header("Settings")]
        [SerializeField][Min(0)] private float _fallOffSpeed = default;
        [SerializeField] [Min(0)] private float _riseUpSpeed = 1f;
        [Space()]
        [SerializeField] [Range(0, 1)] private float _minorTrauma = .3f;
        [SerializeField] [Range(0, 1)] private float _moderateTrauma = .6f;
        [SerializeField] [Range(0, 1)] private float _majorTrauma = .9f;
        #endregion

        #region Properties
        private float _targetValue = 0f;
        #endregion

        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);

        }

        public void Update()
        {
            // Fall Off
            if (_traumaVariable.RuntimeValue > _targetValue)
            {
                _traumaVariable.RuntimeValue -= _fallOffSpeed * Time.deltaTime;
                if (_traumaVariable.RuntimeValue < 0)
                    _traumaVariable.RuntimeValue = 0;
            }
            // Rise Up
            else if (_targetValue > 0)
            {
                _traumaVariable.RuntimeValue += _riseUpSpeed * Time.deltaTime;
                if (_traumaVariable.RuntimeValue >= _targetValue)
                {
                    _traumaVariable.RuntimeValue = _targetValue;
                    _targetValue = 0f;
                }
            }

        }


        public float GetCurrentTrauma() => _traumaVariable.RuntimeValue;
        public void AddTrauma(float value)
        {
            _targetValue += value;
            if (_targetValue > 1)
                _targetValue = 1;
        }
        public void AddTrauma(TraumaLevel level)
        {
            switch (level)
            {
                case TraumaLevel.Minor:
                    AddTrauma(_minorTrauma);
                    break;
                case TraumaLevel.Moderate:
                    AddTrauma(_moderateTrauma);
                    break;
                case TraumaLevel.Major:
                    AddTrauma(_majorTrauma);
                    break;
            }
        }
    }

    public enum TraumaLevel { Minor, Moderate, Major, }
}
