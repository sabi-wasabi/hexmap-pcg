using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PigeonProject
{
    public class SceneChangerSystem
    {
        //TODO: maybe we could make this a singleton class

        #region Static Properties
        public static SceneChangerSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SceneChangerSystem();
                return _instance;
            }
        }
        private static SceneChangerSystem _instance = null;
        #endregion


        private SceneChangerBase _component;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnLoad()
        {
            if (Instance._component == null)
            {
                var empty = new GameObject
                {
                    name = "SceneChangerBase"
                };
                Instance._component = empty.AddComponent<SceneChangerBase>();
                Instance._component.enabled = true;
            }

            var initalSceneHandler = Instance.FindActiveSceneHandler();
            if (initalSceneHandler != null)
                initalSceneHandler.Initialize();
        }


        public bool ChangeToScene(string sceneName, bool terminate = true, bool initialize = true)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == sceneName) return false;

            if (terminate)
            {
                var previousSceneHandler = FindActiveSceneHandler();
                previousSceneHandler?.Terminate();
            }

            try
            {
                _component.StartCoroutine(LoadScene(sceneName, initialize));
            }
            catch(Exception e)
            {
                Perry.LogWarning($"Something went wrong with loading scene {sceneName}: {e.Message}");
                return false;
            }

            return true;
        }


        private IEnumerator LoadScene(string sceneName, bool initialize)
        {
            yield return null;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            if (initialize)
            {
                var newSceneHandler = FindActiveSceneHandler();
                newSceneHandler?.Initialize();
            }
        }

        private SceneHandler FindActiveSceneHandler()
        {
            //RM I think this could potentially be really slow but we'll see

            var activeSceneRoot = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootObject in activeSceneRoot)
            {
                if (rootObject.TryGetComponent(out SceneHandler sceneHandler))
                {
                    return sceneHandler;
                }
            }
            return null;
        }

        public class SceneChangerBase : MonoBehaviour
        {
            private void OnEnable()
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
