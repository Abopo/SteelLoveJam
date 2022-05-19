using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onCrossedFinishLine = default;


    // Start is called before the first frame update
    void Start() {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Ship") {
            Debug.Log("Player hit checkpoint");

            // The player has hit this checkpoint
            Hit();
        }
    }

    void Hit() {
        _onCrossedFinishLine.RaiseEvent();
    }
}
