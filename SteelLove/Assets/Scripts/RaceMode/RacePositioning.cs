using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaceManager))]
public class RacePositioning : MonoBehaviour
{
    private List<GameObject> _positionOrder = new List<GameObject>();
    private List<GameObject> _destroyedOder = new List<GameObject>();

    [Header("Broadcasting On")]
    [SerializeField] private GameObjectsListEventChannelSO _onRacePositionsUpdated = default;
    [SerializeField] private GameObjectsListEventChannelSO _onReportRaceResults = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinished = default;
    [SerializeField] private GameObjectsListEventChannelSO _onSpawnedShips = default;
    [SerializeField] private GameObjectEventChannelSO _onShipFinishedRace = default;
    [SerializeField] private GameObjectEventChannelSO _onShipDestroyed = default;

    private int lockedPositions = 0;
    private bool _raceActive = false;
    private RaceManager _raceManager;

    private void Awake()
    {
        _raceManager = GetComponent<RaceManager>();
    }

    private void OnEnable()
    {
        _onRaceStateEvent.OnEventRaised += StartTracking;
        _onRaceFinished.OnEventRaised += ReportRaceResults;
        _onSpawnedShips.OnEventRaised += SetShips;
        _onShipFinishedRace.OnEventRaised += LockPosition;
        _onShipDestroyed.OnEventRaised += ShipDestroyed;
    }

    private void OnDisable()
    {
        _onRaceStateEvent.OnEventRaised -= StartTracking;
        _onRaceFinished.OnEventRaised -= ReportRaceResults;
        _onSpawnedShips.OnEventRaised -= SetShips;
        _onShipFinishedRace.OnEventRaised -= LockPosition;
        _onShipDestroyed.OnEventRaised -= ShipDestroyed;
    }

    private void Update()
    {
        if (_raceActive)
        {
            GetCurrentOrder();

            // TODO: pass current order out
            _onRacePositionsUpdated.RaiseEvent(_positionOrder);
        }
    }

    public void SetShips(List<GameObject> shipObjs)
    {
        _positionOrder = shipObjs;
    }

    private void StartTracking()
    {
        _raceActive = true;
    }

    private void GetCurrentOrder()
    {
        List<GameObject> lockedIn = new List<GameObject>();
        List<GameObject> stillRacing = new List<GameObject>();
        
        for (int i = 0; i < _positionOrder.Count; ++i)
        {
            if (_positionOrder[i] != null && _positionOrder[i].GetComponent<ShipController>().Health > 0)
            {
                if (i < lockedPositions)
                {
                    lockedIn.Add(_positionOrder[i]);
                }
                else
                {
                    stillRacing.Add(_positionOrder[i]);
                }
            } else {
                int x;
                x = 1;
            }
        }
        
        stillRacing.Sort((x,y) => ComparePositions(x,y));

        _positionOrder.Clear();
        _positionOrder.AddRange(lockedIn);
        _positionOrder.AddRange(stillRacing);
        _positionOrder.AddRange(_destroyedOder);
    }

    private int ComparePositions(GameObject x, GameObject y)
    {
        var xCheckpointTracker = x.GetComponent<CheckpointTracker>();
        var yCheckpointTracker = y.GetComponent<CheckpointTracker>();

        if (xCheckpointTracker.CurLap > yCheckpointTracker.CurLap)
        {
            return -1;
        }
        else if (xCheckpointTracker.CurLap < yCheckpointTracker.CurLap)
        {
            return 1;
        }
        else
        {
            if (xCheckpointTracker.LastPassedCheckpointNumber > yCheckpointTracker.LastPassedCheckpointNumber)
            {
                return -1;
            }
            else if (xCheckpointTracker.LastPassedCheckpointNumber < yCheckpointTracker.LastPassedCheckpointNumber)
            {
                return 1;
            }
            else
            {
                int nextCheckpointInd = xCheckpointTracker.LastPassedCheckpointNumber + 1;
                GameObject nextCheckpoint = _raceManager.Checkpoints[nextCheckpointInd].gameObject;
                float xDist = (nextCheckpoint.transform.position - x.transform.position).magnitude;
                float yDist = (nextCheckpoint.transform.position - y.transform.position).magnitude;

                if (xDist < yDist)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    private void ReportRaceResults()
    {
        lockedPositions = _positionOrder.Count;
        GetCurrentOrder();
        _onReportRaceResults.RaiseEvent(_positionOrder);
    }

    private void LockPosition(GameObject shipObj)
    {
        lockedPositions++;
    }

    private void ShipDestroyed(GameObject shipObj)
    {
        // insert at begining of list so earlier destroyed ships are put in last
        _destroyedOder.Insert(0, shipObj);
    }
}
