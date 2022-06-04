using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private SceneManagerSO _sceneManager = default;
    [SerializeField] private GameSceneSO _breakRoomScene = default;
    [SerializeField] private GameSceneSO _creditsScene = default;
    [SerializeField] private GameSceneSO _trueEndScene = default;
    [SerializeField] private RaceStateSO _RaceStateSO = default;

    [SerializeField] private InputReader _inputReader = default;

    [Header("Broadcasting On")]
    [SerializeField] private GameObjectEventChannelSO _onLapFinished = default;
    [SerializeField] private GameObjectEventChannelSO _onShipFinishedRace = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onCountdownFinished = default;
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinished = default;
    [SerializeField] private GameObjectsListEventChannelSO _onSpawnedShips = default;
    [SerializeField] private VoidEventChannelSO _onPauseEvent = default;
    [SerializeField] private GameObjectEventChannelSO _onShipDestroyed = default;
    [SerializeField] private GameObjectsListEventChannelSO _onReportRaceResults = default;

    public Checkpoint[] Checkpoints => _checkpoints;
    [SerializeField] private Checkpoint[] _checkpoints;
    private int _activeCheckpoint = 0;

    private void OnEnable()
    {
        _onCountdownFinished.OnEventRaised += OnCountdownFinished;
        _onCrossedNextCheckpoint.OnEventRaised += OnCrossedCheckpoint;
        _onRaceFinished.OnEventRaised += OnRaceFinished;
        _onSpawnedShips.OnEventRaised += OnSpawnedShips;
        _onPauseEvent.OnEventRaised += OnPause;
        _onShipDestroyed.OnEventRaised += OnShipDestroyed;
        _onReportRaceResults.OnEventRaised += RewardPoints;

        // input events
        _inputReader.PauseEvent += OnPause;
        _inputReader.RaceSkipEvent += OnRaceSkip;
        _inputReader.PostRaceSkipEvent += ConfirmEndOfRace;
    }

    private void OnDisable()
    {
        _onCountdownFinished.OnEventRaised -= OnCountdownFinished;
        _onCrossedNextCheckpoint.OnEventRaised -= OnCrossedCheckpoint;
        _onRaceFinished.OnEventRaised -= OnRaceFinished;
        _onSpawnedShips.OnEventRaised -= OnSpawnedShips;
        _onPauseEvent.OnEventRaised -= OnPause;
        _onShipDestroyed.OnEventRaised -= OnShipDestroyed;
        _onReportRaceResults.OnEventRaised -= RewardPoints;

        // input events
        _inputReader.PauseEvent -= OnPause;
        _inputReader.ConfirmEvent -= ConfirmEndOfRace;
        _inputReader.RaceSkipEvent -= OnRaceSkip;
        _inputReader.PostRaceSkipEvent -= ConfirmEndOfRace;
    }

    private void Start()
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.None);
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Initializing);
        Time.timeScale = 1.0f;
        InitCheckpoints();
    }

    private void InitCheckpoints()
    {
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            _checkpoints[i].Init(i);
        }
    }

    private void OnCountdownFinished()
    {
        if (_RaceStateSO.CurrentState == RaceStateSO.RaceState.Pause)
        {
            return;
        }
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Race);

        // Activate first checkpoint
        _checkpoints[_activeCheckpoint].Activate();
    }

    private void OnCrossedCheckpoint(GameObject shipObj)
    {
        if(AllCheckpointsCrossed(shipObj)) {
            bool isPlayer = shipObj.GetComponent<PlayerShipSetup>() != null;

            if (isPlayer)
            {
                ResetCheckpoints();
            }

            CheckpointTracker checkpointTracker = shipObj.GetComponent<CheckpointTracker>();

            // If lap 3, end race
            if (checkpointTracker.CurLap >= 2 && checkpointTracker.FinishedRace == false) {
                if (isPlayer)
                {
                    _RaceStateSO.UpdateState(RaceStateSO.RaceState.RaceFinished);
                }
                _onShipFinishedRace.RaiseEvent(shipObj);
            }
            _onLapFinished.RaiseEvent(shipObj);
        }
    }

    private void OnSpawnedShips(List<GameObject> shipObjs)
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Countdown);
    }

    private void OnShipDestroyed(GameObject ship)
    {
        if(ship.GetComponent<PlayerShipSetup>() != null)
        {
            _RaceStateSO.UpdateState(RaceStateSO.RaceState.RaceFinished);
        }
    }

    private void OnRaceFinished() {
        // Stop ships momentum?

        // Display race finished message

        // Let GameManager handle end of race
        if (GameManager.instance != null && _sceneManager.previousScene == _breakRoomScene) {
            GameManager.instance.RaceFinished();
        }
    }

    private void ConfirmEndOfRace()
    {
        if(_RaceStateSO.CurrentState == RaceStateSO.RaceState.RaceFinished)
        {
            if (_sceneManager.previousScene == _breakRoomScene && GameManager.instance.NextRace >= 5) {
                // Load game end stuff

                // If ziv is in first, load True ending
                if (GameManager.instance.GetZivPosition() == 0) {
                    _sceneManager.LoadScene(_trueEndScene);
                } else {
                    // Else, load credits
                    _sceneManager.LoadScene(_creditsScene);
                }

            } else {
                // send us to the previous scene. either break room or main menu.
                _sceneManager.LoadPreviousScene();
            }
        }
    }

    private void RewardPoints(List<GameObject> shipObjs)
    {
        _RaceStateSO.RewardPoints(shipObjs);
    }

    bool AllCheckpointsCrossed(GameObject shipObj) {
        bool allCrossed = false;

        CheckpointTracker checkpointTracker = shipObj.GetComponent<CheckpointTracker>();

        if (checkpointTracker.LastPassedCheckpointNumber == _checkpoints.Length - 1)
        {
            allCrossed = true;
        }

        return allCrossed;
    }

    void ResetCheckpoints() {
        foreach (Checkpoint checkpoint in _checkpoints) {
            checkpoint.Reset();
        }

        // Activate first checkpoint
        _activeCheckpoint = 0;
        _checkpoints[_activeCheckpoint].Activate();
    }

    private void OnPause()
    {
        if (_RaceStateSO.CurrentState == RaceStateSO.RaceState.Pause)
        {
            _RaceStateSO.ReturnToPreviousState();
            Time.timeScale = 1;
        }
        else
        {
            _RaceStateSO.UpdateState(RaceStateSO.RaceState.Pause);
            Time.timeScale = 0;
        }
    }

    private void OnRaceSkip() {
        // Just immediately end the race
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.RaceFinished);
    }
}
