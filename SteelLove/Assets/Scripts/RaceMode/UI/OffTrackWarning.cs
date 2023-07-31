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
            var playerShipObj = FindObjectOfType<PlayerShipSetup>();
            if (playerShipObj != null)
            {
                _playerTrackDamage = playerShipObj.GetComponent<TrackDamage>();
            }
            else
            {
                Debug.LogWarning("player ship not found");
                return;
            }
        }

        if (_playerTrackDamage.IsOffTrack()) {
            _warning.enabled = true;
        } else {
            _warning.enabled = false;
        }
    }
}
