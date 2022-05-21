using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSun : MonoBehaviour {

    public float fillSpeed;

    [SerializeField] float facingAngle = -0.95f;
    [SerializeField] float dotResult;

    [SerializeField] List<ShipController> _shipsInRange = new List<ShipController> ();

    ParticleSystem _particles;

    // Start is called before the first frame update
    void Start() {
        _particles = GetComponentInChildren<ParticleSystem> ();  
    }

    // Update is called once per frame
    void Update() {
        bool anyShips = false;
        foreach (ShipController ship in _shipsInRange) {
            // Check if the ship is facing us
            if (ship != null && IsFacingSun(ship)) {
                // Fill it's boost
                ship.RefillBoost(fillSpeed);

                anyShips = true;
            }
        }

        // If there were any ships facing us for boost
        if (anyShips) {
            // Play the particles if they aren't
            if (!_particles.isPlaying) {
                _particles.Play();
            }
        } else {
            _particles.Stop();
        }
    }

    bool IsFacingSun(ShipController ship) {
        // TODO: will need to check if ship is much higher or lower than the sun

        bool _isFacing = false;

        // Get vector from ship to sun
        Vector3 toSun = ship.transform.position - transform.position;
        
        // Debugging
        Debug.DrawRay(transform.position, toSun, Color.yellow);
        Debug.DrawRay(ship.transform.position, ship.transform.up, Color.blue);

        toSun.Normalize();
        // Check it's angle with the facing of the ship
        dotResult = Vector3.Dot(ship.transform.forward, toSun);

        if (dotResult < facingAngle) {
            _isFacing = true;
        }

        return _isFacing;
    }

    void OnTriggerEnter(Collider other) {
        //Debug.Log("Trigger enter sun.");
        if(other.tag == "Ship") {
            _shipsInRange.Add(other.GetComponentInParent<ShipController>());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Ship") {
            _shipsInRange.Remove(other.GetComponentInParent<ShipController>());
        }
    }
}
