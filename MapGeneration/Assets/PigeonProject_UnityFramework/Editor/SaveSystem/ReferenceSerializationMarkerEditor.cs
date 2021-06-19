using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(ReferenceSerializationMarker))]
    public class ReferenceSerializationMarkerEditor : Editor
    {
        SerializedProperty _prefabProp = default;
        SerializedProperty _guidProp = default;

        ReferenceSerializationMarker _target = default;

        private void OnEnable()
        {
            _prefabProp = serializedObject.FindProperty("_prefab");
            _guidProp = serializedObject.FindProperty("_guid");
            _target = target as ReferenceSerializationMarker;
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
                    _target.GenerateId();
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                    EditorPigeonUtility.RefreshInspector(target);
                }
            }
            else
            {
                EditorGUILayout.LabelField(_guidProp.stringValue, EditorStyles.boldLabel);
                EditorGUILayout.Space();
                if (GUILayout.Button("Notify gameobject"))
                {
                    (target as ReferenceSerializationMarker).NotifyGameObject();
                }
                EditorGUILayout.Space();
                if (GUILayout.Button("Locate prefab"))
                {
                    Selection.activeObject = ReferenceSerializer.Instance.GetObject(_target.GenerateId());
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
