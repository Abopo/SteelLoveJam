using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShipAI : MonoBehaviour {

    ShipController _ship;

    [SerializeField] AICheckpoint _firstCheckpoint;
    [SerializeField] AICheckpoint _nextCheckpoint;

    [SerializeField] Vector3 _toCheckpoint;
    [SerializeField] Vector2 _input;

    private void Awake() {
        _ship = GetComponent<ShipController>();
    }
    // Start is called before the first frame update
    void Start() {
        _nextCheckpoint = _firstCheckpoint;
    }

    // Update is called once per frame
    void Update() {
        // Force proper Y rotation
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);

        // Aim joystick towards the next checkpoint
        _toCheckpoint = _nextCheckpoint.center - transform.position;
        _toCheckpoint.Normalize();
        _input.x = _toCheckpoint.x;

        // Have to swap to tracking x position if we've rotated
        if (Mathf.Abs(transform.localEulerAngles.x) > 45) {
            _input.y = _toCheckpoint.y;
        } else {
            _input.y = _toCheckpoint.z;
        }

        SendInputToShip();
    }

    // Send input to ship
    void SendInputToShip() {
        if (_input.y < 0) {
            _ship.ThrustForward(Mathf.Abs(_input.y));
        } else {
            _ship.ThrustBackwards(Mathf.Abs(_input.y));
        }

        if (_input.x > 0) {
            _ship.ThrustRight(Mathf.Abs(_input.x));
        } else {
            _ship.ThrustLeft(Mathf.Abs(_input.x));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "CheckpointAI") {
            if (other.GetComponent<AICheckpoint>().nextCheckpoint.id > _nextCheckpoint.id ||
                other.GetComponent<AICheckpoint>().nextCheckpoint.id == 0) {
                _nextCheckpoint = other.GetComponent<AICheckpoint>().nextCheckpoint;
            }
        }
    }
}
