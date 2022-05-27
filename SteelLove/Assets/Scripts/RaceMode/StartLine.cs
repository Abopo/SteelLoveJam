using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    [SerializeField] private List<Transform> _startingPoints;
    [SerializeField] private RaceStateSO _raceStateSO;

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onSpawnedShips = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onInitializeStateEvent = default;

    private void OnEnable()
    {
        _onInitializeStateEvent.OnEventRaised += OnInitialize;
    }

    private void OnDisable()
    {
        _onInitializeStateEvent.OnEventRaised -= OnInitialize;
    }

    private void OnInitialize()
    {
        Debug.Log("init starting line");
        List<CharacterSO> polePosiitons = _raceStateSO.PolePositions;
        for(int i = 0; i < polePosiitons.Count; ++i)
        {
            GameObject ship = Instantiate(polePosiitons[i].ShipPrefab);
            ship.transform.position = _startingPoints[i].position;
            ship.transform.rotation = _startingPoints[i].rotation;
        }

        _onSpawnedShips.RaiseEvent();
    }
}
