using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffTrackWarning : MonoBehaviour {

    TrackDamage _playerTrackDamage;
    Image _warning;

    private void Awake() {
        _warning = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start() {
        _warning.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (_playerTrackDamage == null) {
            _playerTrackDamage = FindObjectOfType<PlayerShipSetup>().GetComponent<TrackDamage>();
        }

        if (_playerTrackDamage.IsOffTrack()) {
            _warning.enabled = true;
        } else {
            _warning.enabled = false;
        }
    }
}
