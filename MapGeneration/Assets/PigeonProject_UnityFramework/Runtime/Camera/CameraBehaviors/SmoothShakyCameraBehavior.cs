using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PigeonProject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PigeonProject
{
    [CreateAssetMenu(menuName = "PigeonProject/Camera/StaticCameraBehavior")]
    public class SmoothShakyCameraBehavior : BaseCameraBehavior
    {
        // Todo: this might get moved or deleted
        public enum AnimationFunction
        {
            Sine2DAnimation,
            MultiSine2DAnimation,
            PerlinNoiseAnimation
        }

        #region Settings
        // Todo: find out how to set step size to decimal increment
        [Range(1.0f, 30f)] [SerializeField] private float _frequency;
        [Range(1.0f, 30f)] [SerializeField] private float _amplitude;

        [SerializeField] private AnimationFunction _animationOption;
        private delegate Vector3 AnimFunc(float x, float z, float t);
        private AnimFunc _animFunc;

        private const float PI = Mathf.PI;
        #endregion

        #region Base Functions

        public override Vector3 GetOffset()
        {
            float time = Time.realtimeSinceStartup;


            // Todo: think of a better solution for editor dropdown!
            switch (_animationOption)
            {
                case AnimationFunction.Sine2DAnimation:
                    _animFunc = Sine2DAnimation;
                    break;
                case AnimationFunction.MultiSine2DAnimation:
                    _animFunc = MultiSine2DAnimation;
                    break;
                case AnimationFunction.PerlinNoiseAnimation:
                    _animFunc = PerlinNoiseAnimation;
                    break;
            }

            return _animFunc(_position.x, _position.z, time);
        }

        public override Quaternion GetRotation()
        {
            return Quaternion.Euler(_rotation);
        }

        public override float GetFoV()
        {
            return _fov;
        }

        #endregion

        #region Animations

        private Vector3 Sine2DAnimation(float xPos, float zPos, float t)
        {
            Vector3 p;

            float y = _amplitude * Mathf.Sin(_frequency * PI * (xPos + t));

            p.x = xPos;
            p.y = y + _position.y;
            p.z = zPos;

            return p;
        }

        private Vector3 MultiSine2DAnimation(float xPos, float zPos, float t)
        {
            Vector3 p;

            float y = _amplitude * Mathf.Sin(_frequency * PI * (xPos + 0.5f * t));
            y += 0.5f * _amplitude * Mathf.Sin(_frequency * PI * (xPos + t));

            p.x = xPos;
            p.y = y + _position.y;
            p.z = zPos;

            return p;
        }

        private Vector3 PerlinNoiseAnimation(float xPos, float zPos, float t)
        {
            Vector3 p;

            // Todo: find implicit formular of circle
            float y = _amplitude * Mathf.PerlinNoise((xPos * _frequency + t), (zPos * _frequency + t));

            p.x = xPos;
            p.y = y + _position.y;
            p.z = zPos;

            return p;
        }
        #endregion
    }
}