using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    [SerializeField] private List<Transform> _startingPoints;
    [SerializeField] private RaceStateSO _raceStateSO;

    [Header("Broadcasting On")]
    [SerializeField] private GameObjectsListEventChannelSO _onSpawnedShips = default;

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
        List<CharacterSO> polePositions = _raceStateSO.PolePositions;
        List<GameObject> shipObjs = new List<GameObject>();
        for(int i = 0; i < polePositions.Count; ++i)
        {
            GameObject ship = Instantiate(polePositions[i].ShipPrefab);
            ship.transform.position = _startingPoints[i].position;
            ship.transform.rotation = _startingPoints[i].rotation;
            ship.GetComponent<ShipController>().spawnPoint = _startingPoints[i];
            shipObjs.Add(ship);
        }

        _onSpawnedShips.RaiseEvent(shipObjs);
    }
}
