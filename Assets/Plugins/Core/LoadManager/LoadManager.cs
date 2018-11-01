using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDC.LoadScreen
{
    [AddComponentMenu("TDC/LoadManager")]
    public class LoadManager : MonoBehaviourSingleton<LoadManager>
    {
        public delegate void OnLoad();

        public event OnLoad eventLoad;

        #region Data

        private const string nameSceneLoader = "LoadManager";
        public float progress { get; private set; }

        #endregion

        #region Core

        public void LoadScene(List<int> sceneIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex, mode));
        }

        protected virtual IEnumerator LoadAsynchronously(List<int> sceneIndex,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(nameSceneLoader);

            yield return new WaitForSeconds(0.5f);

            for (var i = 0; i < sceneIndex.Count; i++)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex[i], mode);

                if (operation == null)
                {
                    Debug.Log(Core.Instance.GetStringColor("Scene not found", DDebug.TColor.Error));
                    yield return null;
                }

                while (!operation.isDone)
                {
                    if (operation.progress > 0)
                    {
                        progress = Mathf.Clamp01(operation.progress / .9f) / sceneIndex.Count * (i + 1);
                    }

                    Debug.Log(Core.Instance.DebugLog("Load: " + progress * 100, DDebug.TColor.Warning));

                    yield return null;
                }

                if (progress >= 1)
                {
                    SceneManager.UnloadSceneAsync(nameSceneLoader);
                }

                eventLoad();
            }
        }

        #endregion
    }
}