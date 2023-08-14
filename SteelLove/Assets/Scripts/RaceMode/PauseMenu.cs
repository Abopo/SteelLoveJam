using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _showOnPause;
    [SerializeField] GameObject _restartButton;
    
    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onPauseEvent = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onCountdownStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceStartEvent = default;
    [SerializeField] private VoidEventChannelSO _onPauseStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedEvent = default;

    [SerializeField] private GameSceneSO _trackToLoad;

    [SerializeField] private SceneManagerSO _sceneManager;
    [SerializeField] private GameSceneSO _breakRoomScene;

    private void OnEnable()
    {
        _onCountdownStateEvent.OnEventRaised += UnPause;
        _onRaceStartEvent.OnEventRaised += UnPause;
        _onPauseStateEvent.OnEventRaised += Pause;
        _onRaceFinishedEvent.OnEventRaised += UnPause;
    }

    private void OnDisable()
    {
        _onCountdownStateEvent.OnEventRaised -= UnPause;
        _onRaceStartEvent.OnEventRaised -= UnPause;
        _onPauseStateEvent.OnEventRaised -= Pause;
        _onRaceFinishedEvent.OnEventRaised -= UnPause;
    }

    private void Start()
    {
        _showOnPause.SetActive(false);
        
        if (_sceneManager.previousScene == _breakRoomScene)
        {
            _restartButton.SetActive(false);
        }
    }

    public void Resume()
    {
        _onPauseEvent.RaiseEvent();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void UnPause()
    {
        _showOnPause.SetActive(false);
    }

    private void Pause()
    {
        _showOnPause.SetActive(true);
    }
}
