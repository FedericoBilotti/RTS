using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public static class SceneUtils
    {
        public static Scene SetActiveSceneByIndex(int sceneIndex)
        {
            Scene activeScene = SceneManager.GetSceneByBuildIndex(sceneIndex);

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
            else
            {
                Debug.LogError("Scene with index " + sceneIndex + " not valid!");
            }

            return activeScene;
        }
    }
}