using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private RaceStateSO _RaceStateSO = default;

    [SerializeField] private InputReader _inputReader = default;

    [Header("Broadcasting On")]
    [SerializeField] private IntEventChannelSO _onLapFinished = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onCountdownFinished = default;
    [SerializeField] private VoidEventChannelSO _onCrossedActiveCheckpoint = default;
    [SerializeField] private VoidEventChannelSO _onCrossedFinishLine = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinished = default;
    [SerializeField] private VoidEventChannelSO _onSpawnedShips = default;
    [SerializeField] private VoidEventChannelSO _onPauseEvent = default;

    [SerializeField] int _curLap = 1;

    [SerializeField] private Checkpoint[] _checkpoints;
    private int _activeCheckpoint = 0;

    private void OnEnable()
    {
        _onCountdownFinished.OnEventRaised += OnCountdownFinished;
        _onCrossedActiveCheckpoint.OnEventRaised += OnCrossedActiveCheckpoint;
        _onCrossedFinishLine.OnEventRaised += OnCrossedFinishLine;
        _onRaceFinished.OnEventRaised += OnRaceFinished;
        _onSpawnedShips.OnEventRaised += OnSpawnedShips;
        _onPauseEvent.OnEventRaised += OnPause;

        // input events
        _inputReader.PauseEvent += OnPause;
    }

    private void OnDisable()
    {
        _onCountdownFinished.OnEventRaised -= OnCountdownFinished;
        _onCrossedActiveCheckpoint.OnEventRaised -= OnCrossedActiveCheckpoint;
        _onCrossedFinishLine.OnEventRaised -= OnCrossedFinishLine;
        _onRaceFinished.OnEventRaised -= OnRaceFinished;
        _onSpawnedShips.OnEventRaised -= OnSpawnedShips;
        _onPauseEvent.OnEventRaised -= OnPause;

        // input events
        _inputReader.PauseEvent -= OnPause;
    }

    private void Start()
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Initializing);
        Time.timeScale = 1.0f;
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
        } else {
            // Set finish line as active?
        }
    }

    private void OnCrossedFinishLine()
    {
        if(AllCheckpointsCrossed()) {
            // Reset checkpoints
            ResetCheckpoints();

            // If lap 3, end race
            if (_curLap >= 3) {
                _RaceStateSO.UpdateState(RaceStateSO.RaceState.RaceFinished);
            }
            _onLapFinished.RaiseEvent(_curLap);
            _curLap++;
        }
    }

    private void OnSpawnedShips()
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Countdown);
    }

    private void OnRaceFinished() {
        // Stop ships momentum?

        // Display race finished message

        // Let GameManager handle end of race
        if (GameManager.instance != null) {
            GameManager.instance.RaceFinished();
        }
    }

    bool AllCheckpointsCrossed() {
        bool allCrossed = true;

        foreach (Checkpoint checkpoint in _checkpoints) {
            if (!checkpoint._passed) {
                allCrossed = false;
            }
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
