using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTracker : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private IntEventChannelSO _onLapFinished = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedStateEvent = default;

    [SerializeField] TMP_Text _lapText;

    private bool _raceFinished;

    private void OnEnable()
    {
        _onLapFinished.OnEventRaised += UpdateLap;
        _onRaceFinishedStateEvent.OnEventRaised += OnRaceFinished;
    }

    private void OnDisable()
    {
        _onLapFinished.OnEventRaised -= UpdateLap;
        _onRaceFinishedStateEvent.OnEventRaised -= OnRaceFinished;
    }

    void UpdateLap(int lapFinished) {
        var curLap = lapFinished + 1;
        if (_raceFinished == false)
        {
            _lapText.text = "Lap: " + curLap + "/3";
        }
    }

    private void OnRaceFinished()
    {
        _raceFinished = true;
    }
}
