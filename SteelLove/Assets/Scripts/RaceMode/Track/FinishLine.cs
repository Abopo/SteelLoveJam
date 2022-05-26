using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onCrossedFinishLine = default;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ship") {
            Debug.Log("Player hit finish line.");

            // The player has hit this checkpoint
            Hit();
        }
    }

    void Hit() {
        _onCrossedFinishLine.RaiseEvent();
    }
}
