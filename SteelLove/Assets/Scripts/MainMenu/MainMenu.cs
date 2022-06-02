using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneManagerSO _sceneManager;
    [SerializeField] private GameSceneSO _mainMenuScene;
    [SerializeField] private GameSceneSO _gameStartScene;

    private void Start()
    {
        _sceneManager.SetStartingScene(_mainMenuScene);
    }

    public void StartGame()
    {
        _sceneManager.LoadScene(_gameStartScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
