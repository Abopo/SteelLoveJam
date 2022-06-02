using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_DIFFICULTY { DUMB = 0, EASY, MID, HARD, EXPERT, NUM_DIFFICULTIES }

public class SimpleShipAI : MonoBehaviour {

    public AI_DIFFICULTY difficulty;

    ShipController _ship;

    [SerializeField] AICheckpoint _firstCheckpoint;
    [SerializeField] AICheckpoint _nextCheckpoint;

    [SerializeField] Vector3 _toCheckpoint;
    [SerializeField] Vector2 _input;

    AICheckpoint[] _allCheckpoints;
    List<AICheckpoint> _hitCheckpoints = new List<AICheckpoint>();

    [SerializeField] private VoidEventChannelSO _onRaceStartEvent = default;

    private void Awake() {
        _ship = GetComponent<ShipController>();

        _allCheckpoints = FindObjectsOfType<AICheckpoint>();

        GameObject fc = GameObject.FindGameObjectWithTag("FirstCheckpointAI");
        if (fc != null) {
            _firstCheckpoint = fc.GetComponent<AICheckpoint>();
        }
    }
    // Start is called before the first frame update
    void Start() {
        // TODO: Set this on game start
        difficulty = (AI_DIFFICULTY)Random.Range(0, (int)AI_DIFFICULTY.NUM_DIFFICULTIES);
        SetupViaDifficulty();
    }

    private void OnEnable() {
        _onRaceStartEvent.OnEventRaised += OnRaceStart;
    }

    private void OnDisable() {
        _onRaceStartEvent.OnEventRaised -= OnRaceStart;
    }

    void SetupViaDifficulty() {
        switch(difficulty) {
            case AI_DIFFICULTY.DUMB:
                _ship.AdjustShipParametersAI(0.5f, 0.5f, 0.5f);
                break;
            case AI_DIFFICULTY.EASY:
                _ship.AdjustShipParametersAI(0.7f, 0.7f, 0.7f);
                break;
            case AI_DIFFICULTY.MID:
                _ship.AdjustShipParametersAI(0.85f, 0.85f, 0.85f);
                break;
            case AI_DIFFICULTY.HARD:
                //_ship.AdjustShipParametersAI(0.5f, 0.5f, 0.5f);
                break;
            case AI_DIFFICULTY.EXPERT:
                _ship.AdjustShipParametersAI(1.1f, 1.1f, 1.1f);
                break;
        }
    }

    void OnRaceStart() {
        _nextCheckpoint = _firstCheckpoint;
    }

    // Update is called once per frame
    void Update() {
        if (_nextCheckpoint != null) {
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

            if (Mathf.Abs(_input.x) > Mathf.Abs(_input.y)) {
                _input.y = 0f;
            }

            // Slight randomization to keep ai's apart from each other
            _input.x += Random.Range(-0.1f, 0.1f);
            _input.y += Random.Range(-0.1f, 0.1f);

            SendInputToShip();
        }
    }

    private void LateUpdate() {
        // Force proper Y rotation
        //transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);
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
        if (other.tag == "CheckpointAI" || other.tag == "FirstCheckpointAI") {
            _hitCheckpoints.Add(other.GetComponent<AICheckpoint>());
            _nextCheckpoint = FindClosestCheckpoint();

            // If we've run out of checkpoints 
            if(_nextCheckpoint == null) {
                // Clear the list, and set the first checkpoint again
                _hitCheckpoints.Clear();
                _nextCheckpoint = _firstCheckpoint;
            }
            /*
            if (other.GetComponent<AICheckpoint>().nextCheckpoint.id > _nextCheckpoint.id ||
                other.GetComponent<AICheckpoint>().nextCheckpoint.id == 0) {
                _nextCheckpoint = other.GetComponent<AICheckpoint>().nextCheckpoint;
            }
            */
        }
    }

    AICheckpoint FindClosestCheckpoint() {
        AICheckpoint tempCheckpoint = null;

        float closestDist = 1000f;
        float tempDist = 0;
        foreach(AICheckpoint ac in _allCheckpoints) {
            if (_hitCheckpoints.Contains(ac)) {
                continue;
            } else {
                tempDist = Vector3.Distance(transform.position, ac.center);
                if (tempDist < closestDist) {
                    tempCheckpoint = ac;
                    closestDist = tempDist;
                }
            }
        }

        return tempCheckpoint;
    }
}
