using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

// Should only ever have one race state
//[CreateAssetMenu(fileName = "RaceState", menuName = "Race/RaceState", order = 51)]
public class RaceStateSO : DescriptionBaseSO
{
    public enum RaceState
    {
        Initializing,
        Countdown,
        Race,
        Pause,
        RaceFinished
    }

    public RaceState CurrentState => _currentState;

    [SerializeField] [ReadOnly] private RaceState _currentState;
    [SerializeField] [ReadOnly] private RaceState _previousState;

    public List<CharacterSO> PolePositions => _polePositions;
    [SerializeField] private List<CharacterSO> _polePositions;

    [SerializeField] InputReader _inputReader;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _onInitializeStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onCountdownStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onPauseStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedEvent = default;

    public void Awake()
    {
        _currentState = RaceState.Initializing;
    }

    public void UpdateState(RaceState newState)
    {
        if (newState == CurrentState)
            return;

        _previousState = _currentState;
        _currentState = newState;

        BroadcastStateEntry();
    }

    public void ReturnToPreviousState()
    {
        if (_previousState != RaceState.Initializing)
            return;

        _currentState = _previousState;
        _previousState = RaceState.Initializing;

        BroadcastStateEntry();
    }

    private void BroadcastStateEntry()
    {
        switch (_currentState)
        {
            case RaceState.Initializing:
                _inputReader.DisableAllInput();
                _onInitializeStateEvent.RaiseEvent();
                break;
            case RaceState.Countdown:
                _inputReader.DisableAllInput();
                _onCountdownStateEvent.RaiseEvent();
                break;
            case RaceState.Race:
                _inputReader.DisableAllInput();
                _inputReader.EnableRacingInput();
                _onRaceStateEvent.RaiseEvent();
                break;
            case RaceState.RaceFinished:
                _inputReader.DisableAllInput();
                _onRaceFinishedEvent.RaiseEvent();
                break;
            case RaceState.Pause:
                _inputReader.DisableAllInput();
                _inputReader.EnableUIInput();
                _onPauseStateEvent.RaiseEvent();
                break;
        }
    }
}
