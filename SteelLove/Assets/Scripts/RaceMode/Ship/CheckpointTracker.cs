using UnityEngine;

/// <summary>
/// Tracks when the ship crosses a checkpoint and if its the expected next checkpoint passes the proper event
/// </summary>
public class CheckpointTracker : MonoBehaviour
{
    [Header("Broadcasting On")]
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private GameObjectEventChannelSO _onLapFinished = default;
    [SerializeField] private GameObjectEventChannelSO _onShipFinishedRace = default;

    public int LastPassedCheckpoint => _lastPassedCheckpoint;
    public int CurLap => _curLap;
    public bool FinishedRace => _finishedRace;

    private int _lastPassedCheckpoint = -1;
    private int _curLap = 0;
    private bool _finishedRace;

    private void OnEnable()
    {
        _onRaceStateEvent.OnEventRaised += OnRaceStart;
        _onLapFinished.OnEventRaised += OnLapFinished;
        _onShipFinishedRace.OnEventRaised += OnFinishedRace;
    }

    private void OnDisable()
    {
        _onRaceStateEvent.OnEventRaised -= OnRaceStart;
        _onLapFinished.OnEventRaised -= OnLapFinished;
        _onShipFinishedRace.OnEventRaised += OnFinishedRace;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            var checkpoint = other.gameObject.GetComponent<Checkpoint>();
            if(_lastPassedCheckpoint == checkpoint.CheckpointNumber - 1)
            {
                _lastPassedCheckpoint = checkpoint.CheckpointNumber;
                _onCrossedNextCheckpoint.RaiseEvent(gameObject);
            }
        }
    }

    private void OnRaceStart()
    {
        _lastPassedCheckpoint = -1;
        _curLap = 0;
    }

    private void OnLapFinished(GameObject shipObj)
    {
        if (shipObj == gameObject)
        {
            _lastPassedCheckpoint = -1;
            if (!_finishedRace)
            {
                _curLap++;
            }
        }
    }

    private void OnFinishedRace(GameObject shipObj)
    {
        if (shipObj == gameObject)
        {
            _finishedRace = true;
        }
    }
}
