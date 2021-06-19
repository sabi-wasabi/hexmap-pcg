// The Pigeon Protocol

using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PigeonProject
{
    [Serializable]
    [CreateAssetMenu(menuName = "PigeonProject/ButlerPush/ButlerBuild")]
    public class ButlerBuild : ScriptableObject
    {
        #region Properties
        [Header("Butler Settings")]
        [SerializeField] private string account = "the-pigeon-protocol";
        [SerializeField] private string project = "test-01";
        [SerializeField] private string configuration = "Release";
        [Space()]
        [Tooltip("Optional Id for uploading to steamworks")]
        [SerializeField] private int steamAppId = -1;

        [Header("Scenes")]
        [SerializeField]
        private List<SceneAsset> scenes = new List<SceneAsset>();

        [Header("Build Targets")]
        [SerializeField]
        private List<BuildInfo> buildInfos = new List<BuildInfo>();


        public static ButlerBuild Instance { get; private set; }
        #endregion

        #region Settings
        private const string SOLUTION_DIRECTORY = @".\";
        #endregion

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            UnityEngine.Debug.Log("Initialize Butler Build");
            if (Instance == null)
            {
                string asset = AssetDatabase.FindAssets($"t: {typeof(ButlerBuild).GetTypeInfo().Name}")[0];
                var path = AssetDatabase.GUIDToAssetPath(asset);
                UnityEngine.Debug.Log($"Load instance at path: {path}");
                Instance = AssetDatabase.LoadAssetAtPath(path, typeof(ButlerBuild)) as ButlerBuild;
            }
        }
#endif


        #region Butler Push
        /// <summary>
        /// Builds all versions and pushes them to itch.io.
        /// </summary>
        [MenuItem("ButlerBuild/Butler Push _F6")]
        public static bool ButlerPushAll()
        {
            if (Instance == null)
            {
                UnityEngine.Debug.LogWarning("No ButlerBuild instance set!");
                return false;
            }
            try
            {
                string[] butlerArguments = new string[6];

                butlerArguments[0] = Instance.account;     // Account
                butlerArguments[1] = Instance.project;        // Project
                butlerArguments[3] = Instance.configuration;    // Configuration
                butlerArguments[5] = Instance.steamAppId.ToString(); // Steam App Id

                foreach(BuildInfo build in Instance.buildInfos)
                {
                    string directory = build.Directory;
                    string targetDirectory = $@"bin\{directory}\{Instance.configuration}\";

                    if (Build(directory, build.BuildTarget, butlerArguments[1], build.Suffix, Instance.configuration, build.BuildOptions))
                    {
                        butlerArguments[2] = build.Channel;     // Channel
                        butlerArguments[4] = targetDirectory;   // Build Location
                        StartButler(butlerArguments);
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Failed to execute Butler Push:\n" + e);
                return false;
            }
            return true;
        }
        #endregion

        #region Build
        /// <summary>
        /// Builds a player targeting the given system.
        /// </summary>
        /// <param name="targetName">Name of the target folder.</param>
        /// <param name="targetOS">Targeted system.</param>
        /// <returns></returns>
        public static bool Build(string folderName, BuildTarget targetOS, string executableName, string suffix, string configurationName, BuildOptions buildOptions = BuildOptions.None)
        {
            string path = $"bin/{folderName}/";
            string pathConfig = path + configurationName;
            string pathExecutable = pathConfig + "/" + executableName + suffix;

            string pathBackup = path + "Backup";

            if (Directory.Exists(pathConfig))
            {
                if (configurationName == "Release")
                {
                    if (Directory.Exists(pathBackup))
                        Directory.Delete(pathBackup, true);
                    Directory.Move(pathConfig, pathBackup);
                }
                else
                {
                    Directory.Delete(pathConfig, true);
                }
            }

            string[] scenePaths = new string[Instance.scenes.Count];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenePaths[i] = AssetDatabase.GetAssetOrScenePath(Instance.scenes[i]);
            }

            BuildReport report = BuildPipeline.BuildPlayer(scenePaths, pathExecutable, targetOS, buildOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                UnityEngine.Debug.Log($"Build succeeded: {summary.totalSize} bytes at {summary.outputPath}.");
                return true;
            }
            else if (summary.result == BuildResult.Failed)
            {
                UnityEngine.Debug.Log("Build failed");
            }
            return false;
        }
        #endregion

        #region Utility
        private static void StartButler(string[] butlerArguments)
        {
            Process butler = new Process();
            butler.StartInfo.FileName = File.Exists($"{SOLUTION_DIRECTORY}ButlerPush.bat") ? $"{SOLUTION_DIRECTORY}ButlerPush.bat" : $"{SOLUTION_DIRECTORY}resources/ButlerPush.bat";
            butler.StartInfo.Arguments = CreateArguments(butlerArguments);
            butler.Start();
        }

        private static string CreateArguments(string[] arguments)
        {
            if (arguments.Length < 1) return null;
            string output = "";
            foreach (string arg in arguments)
            {
                output = output + arg + " ";
            }
            return output;
        }
        #endregion
    }
}
