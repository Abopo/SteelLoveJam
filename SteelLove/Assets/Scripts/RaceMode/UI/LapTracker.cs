using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTracker : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedStateEvent = default;
    [SerializeField] private GameObjectEventChannelSO _onLapFinished = default;

    [SerializeField] TMP_Text _lapText;

    private bool _raceFinished;
    private int _displayLap;

    private void OnEnable()
    {
        _onRaceStateEvent.OnEventRaised += OnRaceStart;
        _onRaceFinishedStateEvent.OnEventRaised += OnRaceFinished;
        _onLapFinished.OnEventRaised += UpdateLap;
    }

    private void OnDisable()
    {
        _onRaceStateEvent.OnEventRaised += OnRaceStart;
        _onRaceFinishedStateEvent.OnEventRaised -= OnRaceFinished;
        _onLapFinished.OnEventRaised -= UpdateLap;
    }

    private void OnRaceStart()
    {
        _displayLap = 1;
    }

    void UpdateLap(GameObject shipObj) {
        if (shipObj.GetComponent<PlayerShipSetup>() != null)
        {
            _displayLap++;
            if (_raceFinished == false)
            {
                _lapText.text = "Lap: " + _displayLap + "/3";
            }
        }
    }

    private void OnRaceFinished()
    {
        _raceFinished = true;
    }
}
