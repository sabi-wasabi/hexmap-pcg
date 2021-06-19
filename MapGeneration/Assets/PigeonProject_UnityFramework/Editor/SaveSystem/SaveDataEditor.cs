using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(SaveDataSettings))]
    public class SaveDataEditor : Editor
    {
        //TODO: add access to register keys?

        SaveDataSettings _target = default;

        Vector2 _scrollPos = Vector2.zero;

        private void OnEnable()
        {
            _target = target as SaveDataSettings;
        }

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");

            EditorGUILayout.Space();

            if (GUILayout.Button("Load from file", GUILayout.Width(100)))
            {
                SaveSystem.Instance.LoadSaveData(_target);
            }
            if (GUILayout.Button("Load into game", GUILayout.Width(100)))
            {
                SaveSystem.Instance.LoadGame(_target.Data);
                Debug.Log("Loaded game directly from " + _target.Filename);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset register", GUILayout.Width(100)))
            {
                _target.Data.ResetData();
            }


            EditorGUILayout.Space();

            var data = _target.Data;
            if (data != null)
            {
                string json = JsonUtility.ToJson(data, true);
                GUIContent content = new GUIContent(json);
                var height = GUI.skin.box.CalcHeight(content, EditorGUIUtility.currentViewWidth);

                EditorGUILayout.LabelField("JSON", EditorStyles.boldLabel);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(500));
                {
                    EditorGUILayout.LabelField(json, GUILayout.Height(height));
                } EditorGUILayout.EndScrollView();
            }
        }
    }
}
