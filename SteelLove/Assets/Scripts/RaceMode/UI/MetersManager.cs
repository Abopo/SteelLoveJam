using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetersManager : MonoBehaviour {
    [SerializeField] CharacterSO _player;
    [SerializeField] ShipController _playerShip;

    [SerializeField] ResourceMeter _healthMeter;
    [SerializeField] ResourceMeter _boostMeter;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(0.5f);

        // Get the player's ship???
        ShipController[] allShips = FindObjectsOfType<ShipController>();
        foreach (ShipController ship in allShips) {
            if (ship.Character == _player) {
                _playerShip = ship;
            }
        }

        _healthMeter.maxValue = _playerShip.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
