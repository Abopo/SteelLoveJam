using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_DIFFICULTY { DUMB = 0, EASY, MID, HARD, EXPERT, NUM_DIFFICULTIES }

public class SimpleShipAI : MonoBehaviour {

    public AI_DIFFICULTY difficulty;

    [SerializeField] AICheckpoint _firstCheckpoint;
    [SerializeField] AICheckpoint _nextCheckpoint;

    [SerializeField] float _rotSpeed;

    [SerializeField] float _minTankToBoost;

    [SerializeField] float _nonStraightAngleLimit;
    [SerializeField] int _minDistanceStraightForBoost;

    AICheckpoint[] _allCheckpoints;
    List<AICheckpoint> _hitCheckpoints = new List<AICheckpoint>();

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onRaceStartEvent = default;

    #region InputValues
    [Header("InputValues")]
    [SerializeField] Vector2 _dirInput;
    [SerializeField] float _rotInput;
    [SerializeField] bool _isBoostInput;
    #endregion

    private ShipController _ship;
    private Rigidbody _rigidbody;
    private Vector3 _averageTargetPt;
    private int _defaultLookForward = 4;
    private int _lookForwardAmount;
    private float _randomizeMagnitudeRange = 2.5f;

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

        _lookForwardAmount = _defaultLookForward;
    }
    // Start is called before the first frame update
    void Start() {
        difficulty = _ship.Character.difficulty;

        var sabatoged = _ship.Character.sabotaged;
        if(sabatoged) {
            difficulty = AI_DIFFICULTY.DUMB;
        }

        //difficulty = (AI_DIFFICULTY)Random.Range(0, (int)AI_DIFFICULTY.NUM_DIFFICULTIES);
        //difficulty = AI_DIFFICULTY.EXPERT;
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return null;

        SetupViaDifficulty();
    }

    private void OnEnable() {
        _onRaceStartEvent.OnEventRaised += StartAI;
    }

    private void OnDisable() {
        _onRaceStartEvent.OnEventRaised -= StartAI;
    }

    private void FixedUpdate()
    {
        if (_nextCheckpoint != null && _raceStarted)
        {
            var toAvgPos = _averageTargetPt - transform.position;
            var toAvgPosNorm = toAvgPos.normalized;

            UpdateThrustDir(ref toAvgPosNorm);
            UpdateThrustRot(toAvgPosNorm);

            CheckForBoost();

            _dirInput.y = -1f;

            if (toAvgPos.magnitude < 10)
            {
                FindCloseCheckpoint();
            }

            SendInputToShip();
        }
    }

    public void StartAI()
    {
        _raceStarted = true;
        _nextCheckpoint = _firstCheckpoint;
        CalculateTargetPoint();
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

    private void SetupViaDifficulty() {
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

    private void UpdateThrustDir(ref Vector3 toAvgPosNorm)
    {
        var velocityNorm = _rigidbody.velocity.normalized;
        var angle = Vector3.SignedAngle(velocityNorm, toAvgPosNorm, transform.up);

        if (_rigidbody.velocity.magnitude > 10)
        {
            if (angle > -120 && angle < 120)
            {
                angle *= 1.25f;
                toAvgPosNorm = Quaternion.AngleAxis(angle, transform.up) * toAvgPosNorm;

                _dirInput.x = -angle / 120f;
            }
        }
    }

    private void UpdateThrustRot(Vector3 toAvgPosNorm)
    {
        var orthoToAvgNorm = toAvgPosNorm;
        var up = transform.up;
        Vector3.OrthoNormalize(ref up, ref orthoToAvgNorm);

        Quaternion fromTo = Quaternion.FromToRotation(_rigidbody.transform.forward, orthoToAvgNorm);
        float dot = Vector3.Dot(orthoToAvgNorm, _rigidbody.transform.forward);
        float dir = orthoToAvgNorm.AngleDir(_rigidbody.transform.forward, up);

        _rotInput = 0;
        if (dot > 0)
        {
            _rotInput = dot * dir;
        }
        else if (dot < 0)
        {
            // max input in dir because goal is nearly behind us
            _rotInput = dir;
        }

        var newRot = Quaternion.Lerp(_rigidbody.rotation, fromTo * _rigidbody.rotation, Time.deltaTime * _rotSpeed);
        _rigidbody.MoveRotation(newRot);
    }

    // Send input to ship
    private void SendInputToShip() {
        if (_dirInput.y < 0) {
            _ship.ThrustForward(Mathf.Abs(_dirInput.y));
        } else {
            _ship.ThrustBackwards(Mathf.Abs(_dirInput.y));
        }

        if (_dirInput.x > 0) {
            _ship.ThrustRight(Mathf.Abs(_dirInput.x));
        } else {
            _ship.ThrustLeft(Mathf.Abs(_dirInput.x));
        }

        if (_rotInput != 0)
        {
            Vector2 rotVec = new Vector2(_rotInput, 0f);
            _ship.RotationThrust(rotVec);
        }

        _ship.Boost(_isBoostInput? 0f : 1f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "CheckpointAI" || other.tag == "FirstCheckpointAI") {
            var hitCheckpoint = other.GetComponent<AICheckpoint>();
            _nextCheckpoint = hitCheckpoint.nextCheckpoint;

            if(_nextCheckpoint.lookForwardAmount > 0)
            {
                _lookForwardAmount = _nextCheckpoint.lookForwardAmount;
            } else if (_nextCheckpoint.lookForwardAmount == -1)
            {
                _lookForwardAmount = _defaultLookForward;
            }


            CalculateTargetPoint();
        }
    }

    private void CalculateTargetPoint()
    {
        Vector3 averagedPos = Vector3.zero;
        var curCheckpoint = _nextCheckpoint;

        for (int i = 0; i < _lookForwardAmount; i++)
        {
            averagedPos += curCheckpoint.transform.position;
            curCheckpoint = curCheckpoint.nextCheckpoint;
        }

        _averageTargetPt = averagedPos / _lookForwardAmount;

        // randomize slightly
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        Vector3 randomizeNorm = new Vector3(x, y, z).normalized;
        var up = transform.up;
        Vector3.OrthoNormalize(ref up, ref randomizeNorm);
        float magnitude = Random.Range(0, _randomizeMagnitudeRange);

        _averageTargetPt += randomizeNorm * magnitude;
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

    /// <summary>
    /// Check if we are in a situation where we should use boost
    /// if so set input to boost
    /// </summary>
    private void CheckForBoost()
    {
        if (_ship.BoostTank >= _minTankToBoost &&
            IsLongStraightPath())
        {
            _isBoostInput = true;
        }
        else
        {
            _isBoostInput = false;
        }
    }

    private bool IsLongStraightPath()
    {
        AICheckpoint curCheckpoint = _nextCheckpoint;
        float distanceChecked = 0;
        Vector3 previousForward = _ship.transform.forward;
        float accumulatedAngle = Vector3.Angle(previousForward, _nextCheckpoint.transform.position - _ship.transform.position);
        
        while (distanceChecked < _minDistanceStraightForBoost)
        {
            float distance = (curCheckpoint.transform.position - curCheckpoint.nextCheckpoint.transform.position).magnitude;
            distanceChecked += distance;

            Vector3 nextAngle = curCheckpoint.nextCheckpoint.transform.position - curCheckpoint.transform.position;
            float angleCheck = Vector3.Angle(previousForward, nextAngle);
            accumulatedAngle += angleCheck;

            previousForward = nextAngle;
            curCheckpoint = curCheckpoint.nextCheckpoint;

            if (accumulatedAngle > _nonStraightAngleLimit)
            {
                return false;
            }
        }

        return true;
    }
}
