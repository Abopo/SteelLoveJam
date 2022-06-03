using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

// Should only ever have one race state
[CreateAssetMenu(fileName = "RaceState", menuName = "Race/RaceState", order = 51)]
public class RaceStateSO : DescriptionBaseSO
{
    public enum RaceState
    {
        None,
        Initializing,
        Countdown,
        Race,
        Pause,
        RaceFinished
    }

    public GameSceneSO previousScene;

    public RaceState CurrentState => _currentState;

    [SerializeField] [ReadOnly] private RaceState _currentState;
    [SerializeField] [ReadOnly] private RaceState _previousState;

    public List<CharacterSO> PolePositions => _polePositions;
    [SerializeField] private List<CharacterSO> _polePositions;

    [SerializeField] InputReader _inputReader;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _onInitializeStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onCountdownStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceStartEvent = default;
    [SerializeField] private VoidEventChannelSO _onPauseStateEvent = default;
    [SerializeField] private VoidEventChannelSO _onRaceFinishedEvent = default;

    public void Awake()
    {
        _currentState = RaceState.None;
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
        if (_previousState == RaceState.Initializing)
            return;

        _currentState = _previousState;
        _previousState = RaceState.Initializing;

        BroadcastStateEntry();
    }

    public void RewardPoints(List<GameObject> shipsInOrder)
    {
        for(int i = 0; i < 8; i++)
        {
            var shipObj = shipsInOrder[i];
            var shipObjName = shipObj.name;
            shipObjName = shipObjName.Remove(shipObjName.Length - 7, 7);
            var character = _polePositions.Find(x => x.ShipPrefab.name == shipObjName);
            switch(i)
            {
                case 0:
                    character.seasonPoints += 10;
                    break;
                case 1:
                    character.seasonPoints += 8;
                    break;
                case 2:
                    character.seasonPoints += 6;
                    break;
                case 3:
                    character.seasonPoints += 4;
                    break;
                case 4:
                    character.seasonPoints += 3;
                    break;
                case 5:
                    character.seasonPoints += 2;
                    break;
                case 6:
                    character.seasonPoints += 1;
                    break;
                case 7:
                    character.seasonPoints += 0;
                    break;
            }
        }
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
                _inputReader.EnableGeneralInput();
                _onCountdownStateEvent.RaiseEvent();
                break;
            case RaceState.Race:
                _inputReader.DisableAllInput();
                _inputReader.EnableRacingInput();
                _inputReader.EnableGeneralInput();
                _onRaceStartEvent.RaiseEvent();
                break;
            case RaceState.RaceFinished:
                _inputReader.DisableAllInput();
                _inputReader.EnableUIInput();
                _onRaceFinishedEvent.RaiseEvent();
                break;
            case RaceState.Pause:
                _inputReader.DisableAllInput();
                _inputReader.EnableUIInput();
                _inputReader.EnableGeneralInput();
                _onPauseStateEvent.RaiseEvent();
                break;
        }
    }
}
