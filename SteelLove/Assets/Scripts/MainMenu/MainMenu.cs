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

    public void StartGameEasy()
    {
        GameManager.instance.easyMode = true;
        _sceneManager.LoadScene(_gameStartScene);
    }

    public void StartGameNormal()
    {
        GameManager.instance.easyMode = false;
        _sceneManager.LoadScene(_gameStartScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
