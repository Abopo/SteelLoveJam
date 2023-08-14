using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffTrackWarning : MonoBehaviour {

    TrackDamage _playerTrackDamage;
    Image _warning;

    TrackDamage _playerShipTrackDamage;

    private void Awake() {
        _warning = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start() {
        _warning.enabled = false;

        InitPlayerShipTrackDamage();

        if ( _playerShipTrackDamage == null )
        {
            Debug.LogWarning("Player ship not found");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (_playerTrackDamage.IsOffTrack()) {
            _warning.enabled = true;
        } else {
            _warning.enabled = false;
        }
    }

    private void InitPlayerShipTrackDamage()
    {
        var playerShipObj = FindObjectOfType<PlayerShipSetup>();
        if ( playerShipObj == null)
        {
            return;
        }

        _playerShipTrackDamage = playerShipObj.GetComponent<TrackDamage>();
    }
}
