using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointArrow : MonoBehaviour
{
    [SerializeField] private GameObject _arrowObj;
    [SerializeField] private GameObject _missedCheckpointMessage;

    [Header("Listening To")]
    [SerializeField] private GameObjectEventChannelSO _onActiveCheckpointZoneLeft = default;
    [SerializeField] private GameObjectEventChannelSO _onPlayerCrossedNextCheckpoint = default;

    private bool _isDisplaying;
    private GameObject _targetCheckpoint;

    private void Start()
    {
        _arrowObj.SetActive(false);
        _missedCheckpointMessage.SetActive(false);
    }

    private void OnEnable()
    {
        _onActiveCheckpointZoneLeft.OnEventRaised += CheckpointMissed;
        _onPlayerCrossedNextCheckpoint.OnEventRaised += CrossedCheckpoint;
    }

    private void OnDisable()
    {
        _onActiveCheckpointZoneLeft.OnEventRaised -= CheckpointMissed;
        _onPlayerCrossedNextCheckpoint.OnEventRaised -= CrossedCheckpoint;
    }

    private void Update()
    {
        if (_isDisplaying)
        {
            _arrowObj.transform.LookAt(_targetCheckpoint.transform);
        }
    }

    private void CheckpointMissed(GameObject checkpoint)
    {
        _targetCheckpoint = checkpoint;
        _isDisplaying = true;
        _arrowObj.SetActive(true);
        _missedCheckpointMessage.SetActive(true);
    }

    private void CrossedCheckpoint(GameObject checkpoint)
    {
        _targetCheckpoint = null;
        _isDisplaying = false;
        _arrowObj.SetActive(false);
        _missedCheckpointMessage.SetActive(false);
    }
}
