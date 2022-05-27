using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _showOnPause;

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onPauseEvent = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onCountdownStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceStartEvent = default;
    [SerializeField] private VoidEventChannelSO _onPauseStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedEvent = default;

    private bool _isPaused;

    private void OnEnable()
    {
        _onCountdownStateEvent.OnEventRaised += UnPause;
        _onRaceStartEvent.OnEventRaised += UnPause;
        _onPauseStateEvent.OnEventRaised += TogglePause;
        _onRaceFinishedEvent.OnEventRaised += UnPause;
    }

    private void OnDisable()
    {
        _onCountdownStateEvent.OnEventRaised -= UnPause;
        _onRaceStartEvent.OnEventRaised -= UnPause;
        _onPauseStateEvent.OnEventRaised -= TogglePause;
        _onRaceFinishedEvent.OnEventRaised -= UnPause;
    }

    private void Start()
    {
        _showOnPause.SetActive(false);
    }

    public void Resume()
    {
        _onPauseEvent.RaiseEvent();
        TogglePause();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {

    }

    private void UnPause()
    {
        _isPaused = false;
        _showOnPause.SetActive(false);
    }

    private void Pause()
    {
        _isPaused = true;
        _showOnPause.SetActive(true);
    }
    
    private void TogglePause()
    {
        if(_isPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }
}
