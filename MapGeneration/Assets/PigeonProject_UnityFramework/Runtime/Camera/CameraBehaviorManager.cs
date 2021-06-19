using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PigeonProject
{
    public class CameraBehaviorManager : MonoBehaviour
    {
        #region Settings
        [Header("Events")]
        [SerializeField] private GameEvent _afterSwitchCameraEvent = default;
        [Header( "Center Position" )]
        [SerializeField] private Vector3Variable _centerPosition = default;
        [Header( "Main Scene Camera" )]
        [SerializeField] private Camera _camera = default;
        [Header( "Camera Behaviors" )]
        [SerializeField] private List<BaseCameraBehavior> _cameraBehaviors = default;
        [Header( "Switch Speed" )]
        [Tooltip("How fast the Interpolation progresses")]
        [SerializeField] private float _switchSpeed = default;
        [SerializeField] [Range(0, 179)] private float _switchFov = 20f;


        private BaseCameraBehavior _currentCameraBehavior;
        private BaseCameraBehavior _nextCameraBehavior;
        private float _switchTime = 0.0f;
        private const float EPS = 1E-9f;
        #endregion

        #region Properties
        public Vector3 CameraPosition => CameraOffset;
        public Vector3 CameraOffset => _currentCameraBehavior.GetOffset();
        public Quaternion CameraRotation => _currentCameraBehavior.GetRotation();
        public float CameraFoV => _currentCameraBehavior.GetFoV();
        public Vector3 CenterPosition => _centerPosition.RuntimeValue;
        public Vector3 CurrentCamPos => _currentCameraBehavior.GetOffset();
        public Vector3 NextCamPos => _nextCameraBehavior.GetOffset();
        public float DistanceNextAndCurrentBehavior => Vector3.Distance(CurrentCamPos, NextCamPos);
        #endregion


        #region Unity Methods
        private void Awake()
        {
            var defaultBehavior = _cameraBehaviors[0]; // RandomBehavior;
            //Perry.LogWarning("Initial: " + defaultBehavior.ToString());
            _currentCameraBehavior = defaultBehavior;
            _nextCameraBehavior = defaultBehavior;

            // inital
            _camera.fieldOfView = CameraFoV;
            _camera.transform.localPosition = CameraPosition;
            _camera.transform.rotation = CameraRotation;
        }

        private void Update()
        {
            float distanceCovered = (Time.time - _switchTime) * _switchSpeed;
            float fractionDistance = distanceCovered / DistanceNextAndCurrentBehavior;

            Vector3 camPos;
            float camFov;
            Quaternion camRot;

            if (_nextCameraBehavior != _currentCameraBehavior)
            {
                // Todo: make delegate for easing functions and make this timer based for all rotation, fov and position
                camPos = Vector3.Lerp( CurrentCamPos, NextCamPos, fractionDistance );
                camRot = Quaternion.Lerp(_currentCameraBehavior.GetRotation(), _nextCameraBehavior.GetRotation(), fractionDistance);
                if (fractionDistance < .5)
                    camFov = Mathf.Lerp(_currentCameraBehavior.GetFoV(), _switchFov, fractionDistance);
                else
                    camFov = Mathf.Lerp(_switchFov, _nextCameraBehavior.GetFoV(), fractionDistance);

                if (camPos == NextCamPos)
                {
                    _currentCameraBehavior = _nextCameraBehavior;
                    _afterSwitchCameraEvent.Raise();
                }
            }
            else
            {
                camPos = CameraPosition;
                camFov = CameraFoV;
                camRot = CameraRotation;
            }

            _camera.fieldOfView = camFov;
            _camera.transform.localPosition = camPos;
            _camera.transform.localRotation = camRot;
        }
        #endregion

        #region Events
        public void SwitchCameraBehavior(object param)
        {
            if (_cameraBehaviors.Count <= 1)
                return;

            if (param is bool def)
                _nextCameraBehavior = def ? _cameraBehaviors[0] : GetRandomCameraBehaviour();

            _switchTime = Time.time;


            //Perry.Log( "Current: " + _currentCameraBehavior.ToString() );
            //Perry.Log( "Next: " + _nextCameraBehavior.ToString() );
        }
        #endregion

        #region Utility
        private BaseCameraBehavior GetRandomCameraBehaviour()
        {
            if (_cameraBehaviors.Count <= 1)
                return _currentCameraBehavior;
            BaseCameraBehavior newBehaviour;
            do
            {
                newBehaviour = _cameraBehaviors[Random.Range(0, _cameraBehaviors.Count)];
            } while (newBehaviour == _currentCameraBehavior);
            return newBehaviour;
        }
        #endregion

        #region Easing Functions
        // Todo: implement lerp and other cool stuff
        #endregion
    }
}