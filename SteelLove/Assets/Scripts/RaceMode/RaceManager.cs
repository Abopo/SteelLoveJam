using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
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

        // input events
        _inputReader.PauseEvent += OnPause;
        _inputReader.ConfirmEvent += ConfirmEndOfRace;
    }

    private void OnDisable()
    {
        _onCountdownFinished.OnEventRaised -= OnCountdownFinished;
        _onCrossedNextCheckpoint.OnEventRaised -= OnCrossedCheckpoint;
        _onRaceFinished.OnEventRaised -= OnRaceFinished;
        _onSpawnedShips.OnEventRaised -= OnSpawnedShips;
        _onPauseEvent.OnEventRaised -= OnPause;
        _onShipDestroyed.OnEventRaised -= OnShipDestroyed;

        // input events
        _inputReader.PauseEvent -= OnPause;
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

    private void OnCrossedActiveCheckpoint() 
    {
        _activeCheckpoint++;
        if (_activeCheckpoint < _checkpoints.Length) {
            _checkpoints[_activeCheckpoint].Activate();
        }
    }

    private void OnCrossedCheckpoint(GameObject shipObj)
    {
        if(AllCheckpointsCrossed(shipObj)) {
            // Reset checkpoints
            ResetCheckpoints();

            CheckpointTracker checkpointTracker = shipObj.GetComponent<CheckpointTracker>();

            // If lap 3, end race
            if (checkpointTracker.CurLap >= 2 && checkpointTracker.FinishedRace == false) {
                if (shipObj.GetComponent<PlayerShipSetup>() != null)
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
        if (GameManager.instance != null) {
            GameManager.instance.RaceFinished();
        }
    }

    private void ConfirmEndOfRace()
    {
        if(_RaceStateSO.CurrentState == RaceStateSO.RaceState.RaceFinished)
        {
            // for demo go to main menu
            SceneManager.LoadScene(0);
        }
    }

    bool AllCheckpointsCrossed(GameObject shipObj) {
        bool allCrossed = false;

        CheckpointTracker checkpointTracker = shipObj.GetComponent<CheckpointTracker>();

        if (checkpointTracker.LastPassedCheckpoint == _checkpoints.Length - 1)
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
}
