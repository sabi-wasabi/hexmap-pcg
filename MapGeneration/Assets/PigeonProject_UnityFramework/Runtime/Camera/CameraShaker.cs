using UnityEngine;

namespace PigeonProject
{
    /// <summary>
    /// Applies camera shake based on trauma level.
    /// </summary>
    public class CameraShaker : MonoBehaviour
    {
        #region Settings
        [Header("Settings")]
        [SerializeField] [Min(0)] float _shakePower = 2f;
        [SerializeField] [Range(0, 1)] float _minShake = 0f;

        [Header("Rotational Shake")]
        [SerializeField] bool _applyRotationalShake = false;
        [Space()]
        [SerializeField] [Range(0, 180)] float _maxYaw = 30;
        [SerializeField] [Min(0)] float _frequencyYaw = 1;
        [Space()]
        [SerializeField] [Range(0, 180)] float _maxPitch = 30;
        [SerializeField] [Min(0)] float _frequencyPitch = 1;
        [Space()]
        [SerializeField] [Range(0, 180)] float _maxRoll = 30;
        [SerializeField] [Min(0)] float _frequencyRoll = 1;

        [Header("Translational Shake")]
        [SerializeField] bool _applyTranslationalShake = false;
        [Space()]
        [SerializeField] [Min(0)] float _maxOffsetX = 3;
        [SerializeField] [Min(0)] float _frequencyOffsetX = 1;
        [Space()]
        [SerializeField] [Min(0)] float _maxOffsetY = 3;
        [SerializeField] [Min(0)] float _frequencyOffsetY = 1;
        [Space()]
        [SerializeField] [Min(0)] float _maxOffsetZ = 3;
        [SerializeField] [Min(0)] float _frequencyOffsetZ = 1;
        #endregion

        #region Properties
        private Camera _camera = default;

        private Vector3 _baseCamPosition = default;
        private Vector3 _baseCamRotation = default;

        private bool _isTransitioning = false;

        private readonly float[] _seeds = new float[6];
        #endregion


        private void Start()
        {
            if (!TryGetComponent(out _camera))
            {
                Perry.Log("CameraShaker could not find Camera!");
                enabled = false;
                return;
            }

            // Setup seeds for perlin noise
            for (int i = 0; i < _seeds.Length; i++)
                _seeds[i] = Random.Range(0f, 10000f);

            SetBaseCamera();
        }

        private void LateUpdate()
        {
            if (_isTransitioning)
            {
                SetBaseCamera();
            }

            float shake = CameraTraumaHandler.Instance != null ?
                Mathf.Max(_minShake, Mathf.Pow(CameraTraumaHandler.Instance.GetCurrentTrauma(), _shakePower)) :
                _minShake;


            // Rotational shake
            if (_applyRotationalShake)
            {
                float addYaw =  _maxYaw * shake * MathUtil.GetPerlinNoise(Time.time, min: -1, max: 1, frequency: _frequencyYaw * Time.timeScale, seedX: _seeds[0]);
                float addRoll = _maxRoll * shake * MathUtil.GetPerlinNoise(Time.time, min: -1, max: 1, frequency: _frequencyRoll * Time.timeScale, seedX: _seeds[1]);
                float addPitch = _maxPitch * shake * MathUtil.GetPerlinNoise(Time.time, min: -1, max: 1, frequency: _frequencyPitch * Time.timeScale, seedX: _seeds[2]);

                _camera.transform.localEulerAngles = new Vector3(
                    _baseCamRotation.x + addPitch,
                    _baseCamRotation.y + addYaw,
                    _baseCamRotation.z + addRoll);
            }

            // Translational shake
            if (_applyTranslationalShake)
            {
                float addX = _maxOffsetX * shake * MathUtil.GetPerlinNoise(Time.time, min: -1, max: 1, frequency: _frequencyOffsetX * Time.timeScale, seedX: _seeds[3]);
                float addY = _maxOffsetY * shake * MathUtil.GetPerlinNoise(Time.time, min: -1, max: 1, frequency: _frequencyOffsetY * Time.timeScale, seedX: _seeds[4]);
                float addZ = _maxOffsetZ * shake * MathUtil.GetPerlinNoise(Time.time, min: -1, max: 1, frequency: _frequencyOffsetZ * Time.timeScale, seedX: _seeds[5]);

                _camera.transform.localPosition = new Vector3(
                    _baseCamPosition.x + addX,
                    _baseCamPosition.y + addY,
                    _baseCamPosition.z + addZ);
            }
        }


        public void OnSwitchCameraBehaviour(object _) => _isTransitioning = true;

        public void OnAfterSwitchCameraBehaviour(object _) => _isTransitioning = false;


        private void SetBaseCamera()
        {
            _baseCamPosition = _camera.transform.localPosition;
            _baseCamRotation = _camera.transform.localEulerAngles;
        }

        public void ResetCamera()
        {
            if (_camera == null)
                return;
            _camera.transform.position = _baseCamPosition;
            _camera.transform.eulerAngles = _baseCamRotation;
        }
    }
}
