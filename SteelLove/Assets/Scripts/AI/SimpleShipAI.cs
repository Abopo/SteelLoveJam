using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_DIFFICULTY { DUMB = 0, EASY, MID, HARD, EXPERT, NUM_DIFFICULTIES }

public class SimpleShipAI : MonoBehaviour {

    public AI_DIFFICULTY difficulty;

    ShipController _ship;
    Rigidbody _rigidbody;

    [SerializeField] AICheckpoint _firstCheckpoint;
    [SerializeField] AICheckpoint _nextCheckpoint;

    [SerializeField] float _rotSpeed;

    [SerializeField] Vector2 _input;
    Vector2 _rotInput;

    AICheckpoint[] _allCheckpoints;
    List<AICheckpoint> _hitCheckpoints = new List<AICheckpoint>();

    [SerializeField] private VoidEventChannelSO _onRaceStartEvent = default;

    private Vector3 _averageTargetPt;
    private int _lookForwardAmount = 10;

    private bool _raceStarted = false;

    private void Awake() {
        _ship = GetComponent<ShipController>();
        _rigidbody = GetComponent<Rigidbody>();

        _allCheckpoints = FindObjectsOfType<AICheckpoint>();

        GameObject fc = GameObject.FindGameObjectWithTag("FirstCheckpointAI");
        if (fc != null) {
            _firstCheckpoint = fc.GetComponent<AICheckpoint>();
        }

        BuildCheckpointList();
    }
    // Start is called before the first frame update
    void Start() {
        // TODO: Set this on game start
        difficulty = (AI_DIFFICULTY)Random.Range(0, (int)AI_DIFFICULTY.NUM_DIFFICULTIES);
        difficulty = AI_DIFFICULTY.EXPERT;
        SetupViaDifficulty();
    }

    private void OnEnable() {
        _onRaceStartEvent.OnEventRaised += OnRaceStart;
    }

    private void OnDisable() {
        _onRaceStartEvent.OnEventRaised -= OnRaceStart;
    }

    private void BuildCheckpointList()
    {
        var curCheckpoint = _firstCheckpoint;
        var rayDir = new Vector3(0, 0, -1); // TODO: fix this. shouldnt be hardcoded. might need to be based on transform for first one.

        for (int i = 0; i < _allCheckpoints.Length; i++)
        {
            RaycastHit hit = new RaycastHit();
            int mask = LayerMask.GetMask("CheckpointAI");
            if (Physics.Raycast(curCheckpoint.transform.position, rayDir, out hit, 50, mask))
            {
                var hitCheckpoint = hit.transform.gameObject.GetComponent<AICheckpoint>();
                if (hitCheckpoint)
                {
                    curCheckpoint.nextCheckpoint = hitCheckpoint;
                    curCheckpoint = hitCheckpoint;

                    var ourNormal = rayDir;
                    var nextNormal = -hit.normal;
                    var up = hit.transform.up;
                    Vector3.OrthoNormalize(ref ourNormal, ref up);
                    Vector3.OrthoNormalize(ref nextNormal, ref up);

                    curCheckpoint.angleToNextCheckpoint = Vector3.SignedAngle(ourNormal, nextNormal, transform.up);

                    rayDir = -hit.normal;
                }
                else
                {
                    Debug.Log("failed");
                }
            }
        }
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
        _raceStarted = true;
        _nextCheckpoint = _firstCheckpoint;
        CalculateTargetPoint();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (_nextCheckpoint != null && _raceStarted) {
            // Aim joystick towards the next checkpoint
            var toAvgPos = _averageTargetPt - transform.position;
            var toAvgPosNorm =  toAvgPos.normalized;

            var velocityNorm = _rigidbody.velocity.normalized;
            var angle = Vector3.SignedAngle(velocityNorm, toAvgPosNorm, transform.up);

            if (_rigidbody.velocity.magnitude > 10)
            {
                if (angle > -120 && angle < 120)
                {
                    angle *= 1.5f;
                    toAvgPosNorm = Quaternion.AngleAxis(angle, transform.up) * toAvgPosNorm;

                    _input.x = -angle / 120f;
                }
            }

            var orthoToAvgNorm = toAvgPosNorm;
            var up = transform.up;
            Vector3.OrthoNormalize(ref up, ref orthoToAvgNorm);

            Quaternion fromTo = Quaternion.FromToRotation(_rigidbody.transform.forward, orthoToAvgNorm);
            var newRot = Quaternion.Lerp(_rigidbody.rotation, fromTo * _rigidbody.rotation, Time.deltaTime * _rotSpeed);
            _rigidbody.MoveRotation(newRot);

            //var facingAngle = Vector3.SignedAngle(transform.forward, toAvgPosNorm, transform.up);

            //_rigidbody.rotation *= Quaternion.Euler(transform.up * facingAngle);

            //if (facingAngle < 25 && facingAngle > -25)
            //{
            //    _rotInput.x = Mathf.Clamp(facingAngle / 360f, -1f, 1f);
            //}
            //else
            //{
            //    _rotInput.x = Mathf.Clamp(facingAngle / 180f, -1f, 1f);
            //}

            //transform.rotation *= Quaternion.Euler(_attachToTrack.LatestNormal * facingAngle);

            //transform.LookAt(transform.position + toAvgPosNorm, _attachToTrack.LatestNormal);

            _input.y = -1f;

            if (toAvgPos.magnitude < 10)
            {
                FindCloseCheckpoint();
            }

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

        //if (_rotInput.x != 0)
        //{
        //    _ship.RotationThrust(_rotInput);
        //}
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "CheckpointAI" || other.tag == "FirstCheckpointAI") {
            var hitCheckpoint = other.GetComponent<AICheckpoint>();
            _nextCheckpoint = hitCheckpoint.nextCheckpoint;

            if(_nextCheckpoint.lookForwardAmount > 0)
            {
                _lookForwardAmount = _nextCheckpoint.lookForwardAmount;
            }


            CalculateTargetPoint();
        }
    }

    private void CalculateTargetPoint()
    {
        Vector3 averagedPos = Vector3.zero;
        var curCheckpoint = _nextCheckpoint;

        int forwardThinking = 7;

        for (int i = 0; i < forwardThinking; i++)
        {
            averagedPos += curCheckpoint.transform.position;
            curCheckpoint = curCheckpoint.nextCheckpoint;
        }

        _averageTargetPt = averagedPos / forwardThinking;
    }

    private void FindCloseCheckpoint()
    {
        for (int i = 0; i < _allCheckpoints.Length; ++i)
        {
            var dist = (_allCheckpoints[i].transform.position - transform.position).magnitude;
            if (dist < 10)
            {
                _nextCheckpoint = _allCheckpoints[i];
                CalculateTargetPoint();
                return;
            }
        }
    }

    private Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}
