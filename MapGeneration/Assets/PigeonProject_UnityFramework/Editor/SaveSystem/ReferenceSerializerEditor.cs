using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(ReferenceSerializer))]
    public class ReferenceSerializerEditor : Editor
    {
        SerializedProperty _keysProp = default;
        SerializedProperty _valuesProp = default;

        SerializedProperty _scripoKeysProp = default;
        SerializedProperty _scripoValuesProp = default;

        private void OnEnable()
        {
            _keysProp = serializedObject.FindProperty("_keys");
            _valuesProp = serializedObject.FindProperty("_values");
            _scripoKeysProp = serializedObject.FindProperty("_scripoKeys");
            _scripoValuesProp = serializedObject.FindProperty("_scripoValues");
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Rebuild references"))
            {
                (target as ReferenceSerializer).BuildReferences();
                EditorUtility.SetDirty(target);
                EditorPigeonUtility.RefreshInspector(target);
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("GameObject References", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            {
                int size = _keysProp.arraySize;
                for (int i=0; i<size; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(_keysProp.GetArrayElementAtIndex(i).stringValue, GUILayout.ExpandWidth(true));

                        var referenceObject = _valuesProp.GetArrayElementAtIndex(i).objectReferenceValue;
                        bool isValid = referenceObject != null;
                        string display = isValid ? referenceObject.name : "invalid";
                        var baseColor = GUI.contentColor;
                        if (!isValid)
                            GUI.contentColor = Color.yellow;
                        EditorGUILayout.LabelField(display, GUILayout.Width(50), GUILayout.ExpandWidth(true));
                        if (!isValid)
                            GUI.contentColor = baseColor;

                        if (GUILayout.Button("Locate", GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                        {
                            Selection.activeObject = referenceObject;
                        }
                    } EditorGUILayout.EndHorizontal();
                }
            }EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("ScriptableObject References", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            {
                int size = _scripoKeysProp.arraySize;
                for (int i = 0; i < size; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(_scripoKeysProp.GetArrayElementAtIndex(i).stringValue, GUILayout.ExpandWidth(true));
                        
                        var referenceObject = _scripoValuesProp.GetArrayElementAtIndex(i).objectReferenceValue;
                        bool isValid = referenceObject != null;
                        string display = isValid ? referenceObject.name : "invalid";
                        var baseColor = GUI.contentColor;
                        if (!isValid)
                            GUI.contentColor = Color.yellow;
                        EditorGUILayout.LabelField(display, GUILayout.Width(50), GUILayout.ExpandWidth(true));
                        if (!isValid)
                            GUI.contentColor = baseColor;

                        if (GUILayout.Button("Locate", GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                        {
                            Selection.activeObject = referenceObject;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
