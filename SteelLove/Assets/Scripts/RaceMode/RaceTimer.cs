using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    [Header("Broadcasting On")]
    [SerializeField] private FloatEventChannelSO _onPostLapTime = default;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private IntEventChannelSO _onLapFinished = default;

    private float _curTime;
    private List<float> _lapTimes = new List<float>();
    private bool _startedRace;

    private void Awake()
    {
        _curTime = 0;
    }

    private void OnEnable()
    {
        _onRaceStateEvent.OnEventRaised += OnRaceStart;
        _onLapFinished.OnEventRaised += OnLapIncreased;
    }

    private void OnDisable()
    {
        _onRaceStateEvent.OnEventRaised -= OnRaceStart;
        _onLapFinished.OnEventRaised -= OnLapIncreased;
    }

    private void Update()
    {
        if (_startedRace)
        {
            _curTime += Time.deltaTime;
        }
    }

    private void OnRaceStart()
    {
        _startedRace = true;
    }

    private void OnLapIncreased(int lap)
    {
        int lapInd = lap - 1; // subtract 1 because we finished the previous lap and 1 for 0 indexing
        var lapTime = _curTime;
        if(lap > 1)
        {
            float prevTimes = 0;
            foreach(var time in _lapTimes)
            {
                prevTimes += time;
            }
            lapTime -= prevTimes;
        }
        _lapTimes.Add(lapTime);

        _onPostLapTime.RaiseEvent(lapTime);
    }
}
