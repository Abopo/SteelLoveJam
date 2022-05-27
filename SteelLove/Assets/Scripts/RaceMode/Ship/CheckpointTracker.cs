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

    public int LastPassedCheckpoint => _lastPassedCheckpoint;
    private int _lastPassedCheckpoint = -1;

    private void OnEnable()
    {
        _onRaceStateEvent.OnEventRaised += ResetCheckpoint;
        _onLapFinished.OnEventRaised += CheckForReset;
    }

    private void OnDisable()
    {
        _onRaceStateEvent.OnEventRaised -= ResetCheckpoint;
        _onLapFinished.OnEventRaised -= CheckForReset;
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

    private void ResetCheckpoint()
    {
        _lastPassedCheckpoint = -1;
    }

    private void CheckForReset(GameObject shipObj)
    {
        if (shipObj = gameObject)
        {
            ResetCheckpoint();
        }
    }
}
