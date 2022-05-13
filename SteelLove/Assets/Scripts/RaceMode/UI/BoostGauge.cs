using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostGauge : MonoBehaviour {

    private TMPro.TMP_Text _UIText;

    [SerializeField] ShipController _playerShip;

    // Start is called before the first frame update
    void Start() {
        _UIText = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update() {
        _UIText.text = "Boost: " + _playerShip.BoostTank;
    }
}
