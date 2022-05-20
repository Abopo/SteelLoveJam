using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private RaceStateSO _RaceStateSO = default;

    [SerializeField] private InputReader _inputReader = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onCountdownFinished = default;
    [SerializeField] private VoidEventChannelSO _onCrossedFinishLine = default;

    private void OnEnable()
    {
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.Initializing);

        _onCountdownFinished.OnEventRaised += OnCountdownFinished;
        _onCrossedFinishLine.OnEventRaised += OnCrossedFinishLine;

        // input events
        _inputReader.PauseEvent += OnPause;
    }

    private void OnDisable()
    {
        _onCountdownFinished.OnEventRaised -= OnCountdownFinished;
        _onCrossedFinishLine.OnEventRaised -= OnCrossedFinishLine;

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

    private void OnCrossedFinishLine()
    {
        Debug.Log("crossed finish");
        _RaceStateSO.UpdateState(RaceStateSO.RaceState.RaceFinished);
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
