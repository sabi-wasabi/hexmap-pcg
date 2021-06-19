// The Pigeon Protocol

using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CreateAssetMenu(menuName = "PigeonProject/ButlerPush/BuildInfo")]
    public class BuildInfo : ScriptableObject
    {
        [Header("Build Settings")]

        [SerializeField] private BuildTarget buildTarget = BuildTarget.StandaloneWindows;
        [Tooltip(".exe for windows; .app for osx; empty for linux")]
        [SerializeField] private string suffix = ".exe";
        [SerializeField] private string channel = "windows-test";
        [SerializeField] private string directory = "Windows";
        [Space(5f)]
        [SerializeField] private BuildOptions buildOptions = BuildOptions.None;

        public BuildTarget BuildTarget { get => buildTarget; }
        public string Suffix { get => suffix; }
        public string Channel { get => channel; }
        public string Directory { get => directory; }
        public BuildOptions BuildOptions { get => buildOptions; }
    }
}
