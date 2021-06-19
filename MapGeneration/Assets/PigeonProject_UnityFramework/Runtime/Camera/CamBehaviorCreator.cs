using System;
using UnityEditor;
using UnityEngine;

namespace PigeonProject
{
    public class CamBehaviorCreator : MonoBehaviour
    {
        private enum BehaviorType
        {
            StaticCameraBehavior,
            SmoothShakyCameraBehavior
        }

        #region Settings
        [SerializeField] private string _name = default;
        [SerializeField] private BehaviorType _behavior = default;
        private const string PATH = "Assets/Variables/_Camera/Cameras/";
        #endregion

        public void CreateBehavior()
        {
            var (fileName, behaviorType) = Validate();

            var behaviorScripo = ScriptableObject.CreateInstance(behaviorType);
            var behavior = behaviorScripo as BaseCameraBehavior;
            var cam = GetComponent<Camera>();

            if (behavior == null)
                throw new Exception("Bomboclaat something went wrong!");

            behavior.InitPosition = cam.transform.position;
            behavior.InitRotation = EulerToInspectorEuler(cam.transform.eulerAngles);
            behavior.InitFoV = cam.fieldOfView;

#if UNITY_EDITOR
            AssetDatabase.CreateAsset( behaviorScripo, PATH + fileName + ".asset");
#endif

            Perry.Log("Saved new Behavior! - " + fileName);

            _name = "";
        }

        #region Helper
        private Vector3 EulerToInspectorEuler(Vector3 euler)
        {
            // This blackmagic gives you the inspector rotation values
            // don't ask me where i got this...
            // this little problem took me 3 hours to solve / even find
            Vector3 angle = euler;
            float x = angle.x;
            float y = angle.y;
            float z = angle.z;

            if (Vector3.Dot( transform.up, Vector3.up ) >= 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = angle.x;
                }
                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = angle.x - 360f;
                }
            }
            if (Vector3.Dot( transform.up, Vector3.up ) < 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = 180 - angle.x;
                }
                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = 180 - angle.x;
                }
            }

            if (angle.y > 180)
            {
                y = angle.y - 360f;
            }

            if (angle.z > 180)
            {
                z = angle.z - 360f;
            }

            return new Vector3(x, y, z);
        }

        private (string, Type) Validate()
        {
            if (_name == "")
                throw new ArgumentException( "Hell naaa, this name not good!" );

            // bool fileExists = AssetDatabase.GetMainAssetTypeAtPath( PATH + _name + ".asset" ) != null;

            /*if (fileExists)
                throw new FileLoadException("File already exists!");*/

            Type type = Type.GetType( _behavior.ToString() );

            return (_name, type);
        }
        #endregion
    }
}
