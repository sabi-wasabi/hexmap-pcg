using System.Collections;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(ReferenceMarkedScriptableObject), editorForChildClasses: true)]
    public class ReferenceMarkedScriptableObjectEditor : Editor
    {
        SerializedProperty _guidProp = default;

        private void OnEnable()
        {
            _guidProp = serializedObject.FindProperty("_guid");
        }

        public override void OnInspectorGUI()
        {
            string guid = _guidProp.stringValue;
            if (guid.Length < 1)
            {
                EditorGUILayout.LabelField("Guid not initialized.", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                if (GUILayout.Button("Generate guid"))
                {
                    (target as ReferenceMarkedScriptableObject).GenerateId();
                    EditorPigeonUtility.RefreshInspector(target);
                }
            }
            else
            {
                EditorGUILayout.LabelField(_guidProp.stringValue, EditorStyles.boldLabel);
            }

            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script", "_guid" });    //RM this is an undocumented function and allows me to display the child editor without exposing the guid.

            serializedObject.ApplyModifiedProperties();
        }
    }
}
