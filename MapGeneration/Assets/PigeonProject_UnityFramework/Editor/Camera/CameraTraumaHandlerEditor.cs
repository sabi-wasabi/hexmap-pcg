using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(CameraTraumaHandler), true)]
    public class CameraTraumaHandlerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            var concreteTarget = target as CameraTraumaHandler;

            GUILayout.Space(15f);

            if (GUILayout.Button("Add minor trauma"))
                concreteTarget.AddTrauma(TraumaLevel.Minor);

            if (GUILayout.Button("Add moderate trauma"))
                concreteTarget.AddTrauma(TraumaLevel.Moderate);

            if (GUILayout.Button("Add major trauma"))
                concreteTarget.AddTrauma(TraumaLevel.Major);

        }
    }
}
