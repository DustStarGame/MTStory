using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Farm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        public string startSceneName;

        private void OnEnable() {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable() {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }
        private void Start()
        {
            StartCoroutine(LoadSceneSetActive(startSceneName));
        }

        private void OnTransitionEvent(string SceneToGo, Vector3 posToGo)
        {
            StartCoroutine(Transition(SceneToGo, posToGo));
        }


        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPos">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPos)
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            yield return LoadSceneSetActive(sceneName);
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneByName(sceneName);
                        
            SceneManager.SetActiveScene(newScene);
        }
    }
}
