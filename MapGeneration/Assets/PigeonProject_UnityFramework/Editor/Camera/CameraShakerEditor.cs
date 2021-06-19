using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(CameraShaker))]
    public class CameraShakerEditor : Editor
    {
        bool _showRotational = true;
        bool _showTranslational = true;
        readonly string[] _rotationalSettings = { "Yaw", "Pitch", "Roll" };
        readonly string[] _translationalSettings = { "OffsetX", "OffsetY", "OffsetZ" };

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_shakePower"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minShake"));

            EditorGUILayout.Space();

            DrawSettingField("Rotational");
            EditorGUILayout.Space();
            DrawSettingField("Translational");

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            if (GUILayout.Button("Reset camera"))
                (target as CameraShaker).ResetCamera();
        }

        private void DrawSettingField(string mode)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty($"_apply{mode}Shake"));
            bool applyRotational = serializedObject.FindProperty($"_apply{mode}Shake").boolValue;
            if (applyRotational)
            {
                bool show = default;
                string[] settings = default;

                switch(mode)
                {
                    case "Rotational":
                        show = _showRotational = EditorGUILayout.BeginFoldoutHeaderGroup(_showRotational, "Settings");
                        settings = _rotationalSettings;
                        break;

                    case "Translational":
                        show = _showTranslational = EditorGUILayout.BeginFoldoutHeaderGroup(_showTranslational, "Settings");
                        settings = _translationalSettings;
                        break;
                }

                if (show)
                {
                    foreach (string setting in settings)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_max" + setting));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_frequency" + setting));
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}
