using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(SaveSystem))]
    public class SaveSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            SaveSystem saveSystem = target as SaveSystem;
            if (GUILayout.Button("Save active data to file", GUILayout.Width(150)))
            {
                saveSystem.SaveGame();
            }
            if (GUILayout.Button("Load active into game", GUILayout.Width(150)))
            {
                saveSystem.LoadGame();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Load all saves from file", GUILayout.Width(150)))
            {
                saveSystem.LoadSaveDataAll();
            }
        }
    }
}
