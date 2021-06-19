// The Pigeon Protocol

using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(ButlerBuild))]
    public class ButlerBuildEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(15f);

            if (GUILayout.Button("Butler Push"))
            {
                if (ButlerBuild.ButlerPushAll())
                    Debug.Log("Succesfully initiated Butler Push.");
                else
                    Debug.LogWarning("Failed to initiate Butler Push!");
            }
        }
    }
}
