using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _raceCompleteMessage;
    [SerializeField] private GameObject _shipDestoryedMessage;

    [SerializeField] private VoidEventChannelSO _onRaceFinishedEvent = default;
    [SerializeField] private GameObjectEventChannelSO _onShipDestroyed = default;

    private bool _playerDestroyed;

    private void Start()
    {
        _raceCompleteMessage.SetActive(false);
        _shipDestoryedMessage.SetActive(false);
        _playerDestroyed = false;
    }

    private void OnEnable()
    {
        _onRaceFinishedEvent.OnEventRaised += EnableUI;
        _onShipDestroyed.OnEventRaised += OnShipDestroyed;
    }

    private void OnDisable()
    {
        _onRaceFinishedEvent.OnEventRaised -= EnableUI;
        _onShipDestroyed.OnEventRaised -= OnShipDestroyed;
    }

    private void EnableUI()
    {
        if (_playerDestroyed)
        {
            _shipDestoryedMessage.SetActive(true);
        }
        else
        {
            _raceCompleteMessage.SetActive(true);
        }
    }

    private void OnShipDestroyed(GameObject ship)
    {
        if (ship.GetComponent<PlayerShipSetup>() != null)
        {
            _playerDestroyed = true;
        }
    }
}
