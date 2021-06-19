using System.Collections;
using System.Collections.Generic;
using PigeonProject;
using UnityEngine;

namespace PigeonProject
{
    [CreateAssetMenu( menuName = "PigeonProject/Camera/StaticCameraBehavior" )]
    public class StaticCameraBehavior : BaseCameraBehavior
    {
        #region Settings
        //[Header( "Settings" )]
        #endregion

        public override Vector3 GetOffset()
        {
            return _position;
        }

        public override Quaternion GetRotation()
        {
            return Quaternion.Euler( _rotation );
        }

        public override float GetFoV()
        {
            return _fov;
        }
    }
}