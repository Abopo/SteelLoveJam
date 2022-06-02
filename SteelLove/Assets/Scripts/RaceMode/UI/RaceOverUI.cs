using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _raceCompleteMessage;
    [SerializeField] private GameObject _shipDestoryedMessage;
    [SerializeField] private GameObject _leaderboardSubmission;

    [SerializeField] private VoidEventChannelSO _onRaceFinishedEvent = default;
    [SerializeField] private GameObjectEventChannelSO _onShipDestroyed = default;

    [SerializeField] private SceneManagerSO _sceneManager = default;

    private bool _playerDestroyed;

    private void Start()
    {
        _raceCompleteMessage.SetActive(false);
        _shipDestoryedMessage.SetActive(false);
        _playerDestroyed = false;
    }

    private void OnEnable()
    {
        _onRaceFinishedEvent.OnEventRaised += EnableUI;
        _onShipDestroyed.OnEventRaised += OnShipDestroyed;
    }

    private void OnDisable()
    {
        _onRaceFinishedEvent.OnEventRaised -= EnableUI;
        _onShipDestroyed.OnEventRaised -= OnShipDestroyed;
    }

    private void EnableUI()
    {
        if (_playerDestroyed)
        {
            _shipDestoryedMessage.SetActive(true);
        }
        else
        {
            _raceCompleteMessage.SetActive(true);

            // If we loaded from the main menu
            if(_sceneManager.previousScene.levelName == "MainMenu") {
                LeaderboardCheck();
            }
        }
    }

    private void LeaderboardCheck() {
        // Check the finish time with the saved best time
        string pref = _sceneManager.currentScene.levelName + " BestTime";
        float bestTime = PlayerPrefs.GetInt(pref);
        float totalTime = FindObjectOfType<TimeTracker>().TotalTime;

        // Show leaderboard submission UI if it's faster (and only if we've started from track select?)
        if (totalTime < bestTime) {
            _leaderboardSubmission.SetActive(true);
            _leaderboardSubmission.GetComponent<LeaderboardSubmission>().leaderboardID = ((TrackSceneSO)_sceneManager.currentScene).trackLeaderboardID;

            // Save new best time
            PlayerPrefs.SetInt(pref, (int)totalTime);
        }
    }

    private void OnShipDestroyed(GameObject ship)
    {
        if (ship.GetComponent<PlayerShipSetup>() != null)
        {
            _playerDestroyed = true;
        }
    }
}
