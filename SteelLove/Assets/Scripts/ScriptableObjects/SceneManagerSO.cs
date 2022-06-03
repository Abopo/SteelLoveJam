using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/SceneManagerSO")]
public class SceneManagerSO : DescriptionBaseSO
{
    public GameSceneSO currentScene;
    public GameSceneSO previousScene;

    /// <summary>
    /// should only be called by the mainMenu passing in mainMenu scene
    /// </summary>
    /// <param name="startScene"></param>
    public void SetStartingScene(GameSceneSO startScene) {
        currentScene = startScene;
    }

    public void LoadScene(GameSceneSO newScene) {
        previousScene = currentScene;
        currentScene = newScene;
        newScene.sceneReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single, true);
    }

    public void LoadScene(GameSceneSO newScene, bool savePrevious) {
        if(savePrevious) {
            previousScene = currentScene;
        }
        currentScene = newScene;
        newScene.sceneReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single, true);
    }

    public void LoadPreviousScene()
    {
        if (previousScene != null)
        {
            currentScene = previousScene;
            previousScene = null;
            currentScene.sceneReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }
        else
        {
            Debug.Log("tried to load previous scene when there was none");
        }
    }
}
