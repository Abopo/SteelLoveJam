using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTracker : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private FloatEventChannelSO _onLapIncreased = default;

    [SerializeField] TMP_Text _lapText;

    // Start is called before the first frame update
    void Start() {
        _onLapIncreased.OnEventRaised += OnLapIncreased;
    }

    void OnLapIncreased(float lap) {
        _lapText.text = "Lap: " + (int)lap + "/3";
    }
}
