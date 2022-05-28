using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameSceneSO _gameStartScene;

    public void StartGame()
    {
        _gameStartScene.sceneReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single, true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
