using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onCountdownStateEvent = default;

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onCountownFinished = default;

    [SerializeField] private TMPro.TMP_Text _timerText;

    [SerializeField] private int _countdownTime = 3;
    [SerializeField] private int _holdTextDisplaytime = 1;

    private float _countdownTimer;
    private bool _countingDown = false;

    private void OnEnable()
    {
        // events
        _onCountdownStateEvent.OnEventRaised += StartCountdown;
    }

    private void OnDisable()
    {
        _onCountdownStateEvent.OnEventRaised -= StartCountdown;   
    }

    private void Update()
    {
        if (_countingDown)
        {
            _countdownTimer -= Time.deltaTime;
            int roundedUp = (int)Math.Ceiling(_countdownTimer);
            _timerText.text = roundedUp.ToString();

            if (_countdownTimer <= 0)
            {
                _onCountownFinished.RaiseEvent();
                StartCoroutine(HideAfterTime());
            }
        }
    }

    private void StartCountdown()
    {
        _countdownTimer = _countdownTime;
        _timerText.text = ((int)_countdownTimer).ToString();
        _timerText.gameObject.SetActive(true);
        _countingDown = true;
    }

    private IEnumerator HideAfterTime()
    {
        _timerText.text = "GO!";
        yield return new WaitForSeconds(_holdTextDisplaytime);
        _timerText.gameObject.SetActive(false);
    }
}
