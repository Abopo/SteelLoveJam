using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private RaceStateSO _RaceStateSO = default;

    [SerializeField] private InputReader _inputReader = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onCountdownFinished = default;

    private void OnEnable()
    {
        _onCountdownFinished.OnEventRaised += OnCountdownFinished;

        // input events
        _inputReader.PauseEvent += OnPause;
    }

    private void OnDisable()
    {
        _onCountdownFinished.OnEventRaised -= OnCountdownFinished;

        // input events
        _inputReader.PauseEvent -= OnPause;
    }

    private void Start()
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Countdown);
    }

    private void OnCountdownFinished()
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Race);
    }

    private void OnPause()
    {
        if (_RaceStateSO.CurrentState == RaceStateSO.RaceState.Pause)
        {
            _RaceStateSO.ReturnToPreviousState();
        }
        else
        {
            _RaceStateSO.UpdateState(RaceStateSO.RaceState.Pause);
        }
    }
}
