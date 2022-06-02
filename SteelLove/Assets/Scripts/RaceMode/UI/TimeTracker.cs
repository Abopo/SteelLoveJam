using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _curTimeTrackerText;
    [SerializeField] private List<TMPro.TMP_Text> _lapTimers;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedStateEvent = default;
    [SerializeField] private FloatEventChannelSO _onPostLapTime = default;

    private float _curTime;
    private bool _raceInProgress;
    private int _curLap;

    public float TotalTime => _curTime;

    private void OnEnable()
    {
        _onRaceStateEvent.OnEventRaised += StartTimer;
        _onRaceFinishedStateEvent.OnEventRaised += StopTimer;
        _onPostLapTime.OnEventRaised += PostLapTime;
    }

    private void OnDisable()
    {
        _onRaceStateEvent.OnEventRaised -= StartTimer;
        _onRaceFinishedStateEvent.OnEventRaised -= StopTimer;
        _onPostLapTime.OnEventRaised -= PostLapTime;
    }

    private void Start()
    {
        foreach(var timer in _lapTimers)
        {
            timer.gameObject.SetActive(false);
        }

        _curTimeTrackerText.text = "00:00:00";
    }

    private void Update()
    {
        if (_raceInProgress)
        {
            _curTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(_curTime);
        _curTimeTrackerText.text = time.ToString("mm':'ss': 'ff");
    }

    private void StartTimer()
    {
        _raceInProgress = true;
    }

    private void StopTimer()
    {
        _raceInProgress = false;
    }

    private void PostLapTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _lapTimers[_curLap].text = timeSpan.ToString("mm':'ss': 'ff");
        _lapTimers[_curLap].gameObject.SetActive(true);

        _curLap++;
    }
}
