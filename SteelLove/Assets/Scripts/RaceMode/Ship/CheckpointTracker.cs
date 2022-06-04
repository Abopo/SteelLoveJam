using UnityEngine;

/// <summary>
/// Tracks when the ship crosses a checkpoint and if its the expected next checkpoint passes the proper event
/// </summary>
public class CheckpointTracker : MonoBehaviour
{
    [SerializeField] private float _distanceToCheckpointAssist;

    [Header("Broadcasting On")]
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;
    [SerializeField] private GameObjectEventChannelSO _onPlayerCrossedNextCheckpoint = default;
    [SerializeField] private GameObjectEventChannelSO _onActiveCheckpointZoneLeft = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private GameObjectEventChannelSO _onLapFinished = default;
    [SerializeField] private GameObjectEventChannelSO _onShipFinishedRace = default;

    public int LastPassedCheckpointNumber => _lastPassedCheckpointNumber;
    public Checkpoint LastPassedCheckpoint => _lastPassedCheckpoint;
    public int CurLap => _curLap;
    public bool FinishedRace => _finishedRace;

    [SerializeField] private int _lastPassedCheckpointNumber = -1;
    [SerializeField] private Checkpoint _lastPassedCheckpoint = null;
    private int _curLap = 0;
    private bool _finishedRace;

    private GameObject _nearbyCheckpoint;
    private bool _alertedAssistanceNeeded;

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
        _onShipFinishedRace.OnEventRaised -= OnFinishedRace;
    }

    private void Update()
    {
        if (_nearbyCheckpoint != null && _alertedAssistanceNeeded == false && _finishedRace == false)
        {
            var distToCheckpoint = (_nearbyCheckpoint.transform.position - gameObject.transform.position).magnitude;
            if (distToCheckpoint >= _distanceToCheckpointAssist && GetComponent<PlayerShipSetup>() != null)
            {
                _onActiveCheckpointZoneLeft.RaiseEvent(_nearbyCheckpoint);
                _alertedAssistanceNeeded = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            var checkpoint = other.gameObject.GetComponent<Checkpoint>();
            if(_lastPassedCheckpointNumber == checkpoint.CheckpointNumber - 1)
            {
                _lastPassedCheckpointNumber = checkpoint.CheckpointNumber;
                _lastPassedCheckpoint = checkpoint;
                _onCrossedNextCheckpoint.RaiseEvent(gameObject);
                if (GetComponent<PlayerShipSetup>() != null)
                {
                    _onPlayerCrossedNextCheckpoint.RaiseEvent(gameObject);
                }
                _nearbyCheckpoint = null;
                _alertedAssistanceNeeded = false;
            }
        }
        else if (other.tag == "CheckpointArea")
        {
            var checkpoint = other.gameObject.transform.parent.GetComponent<Checkpoint>();
            if (_lastPassedCheckpointNumber == checkpoint.CheckpointNumber - 1)
            {
                _nearbyCheckpoint = checkpoint.gameObject;
            }
        }
    }

    private void OnRaceStart()
    {
        _lastPassedCheckpointNumber = -1;
        _curLap = 0;
    }

    private void OnLapFinished(GameObject shipObj)
    {
        if (shipObj == gameObject)
        {
            _lastPassedCheckpointNumber = -1;
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
