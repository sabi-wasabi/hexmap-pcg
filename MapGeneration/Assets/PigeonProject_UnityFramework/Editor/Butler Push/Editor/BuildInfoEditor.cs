// The Pigeon Protocol

using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(BuildInfo))]
    public class BuildInfoEditor : Editor
    {
        BuildInfo build;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(15f);

            if (GUILayout.Button("Local Build"))
            {
                BuildLocal();
            }
        }

        private void BuildLocal()
        {
            string directory = build.Directory;
            BuildTarget targetOS = build.BuildTarget;
            string executableName = directory.ToLower() + "Local";
            string suffix = build.Suffix;
            if (ButlerBuild.Build(directory, targetOS, executableName, suffix, "Local", build.BuildOptions))
                Debug.Log($"Succesfully created local build for OS {directory}.");
            else
                Debug.LogWarning($"Failed to create local build for OS {directory}!");
        }

        private void OnEnable()
        {
            build = target as BuildInfo;
        }
    }
}
