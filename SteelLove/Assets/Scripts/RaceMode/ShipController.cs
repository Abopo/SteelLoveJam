using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour {

    [Header("Movement Properties")]
    [SerializeField] private float _mainthrustForce;
    [SerializeField] private float _boostForceMultiplier;
    [SerializeField] private float _reverseThrustForce;
    [SerializeField] private float _horizontalThrustForce;
    [SerializeField] private float _stepForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _extremeDirChangeAngle;
    [SerializeField] private float _extremeDirChangeMult;

    [SerializeField] private float _rotForce;

    [SerializeField] private float _stepTime;
    [SerializeField] private float _speedLimitEnableAfterStepTime;

    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxSpeedBoostModifier;
    [SerializeField] private float _overSpeedLimitSlowdownForce;
    [SerializeField] private float _maxRotSpeed;

    [SerializeField] private float _boostCost;

    [Header("Input")]
    [SerializeField] private InputReader _inputReader = default;

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _OnCrossedFinishLine = default;
    [SerializeField] private FloatEventChannelSO _onHealthLevelChanged = default;
    [SerializeField] private FloatEventChannelSO _onBoostLevelChanged = default;

    [Header("Effects")]
    [SerializeField] ParticleSystem _boostParticles;

    // Ship Resources
    private float _health = 100.0f;
    private float _boostTank = 0f;

    private Rigidbody2D _rigidbody2D = default;

    private float _mainThrusterInputValue;
    private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;
    private bool _brake;
    private bool _boosting;

    private Vector2 _stepDir;
    private bool _speedLimitEnabled = true;

    ShipThrusters _thrusters;

    public float BoostTank { get => _boostTank; }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _thrusters = GetComponentInChildren<ShipThrusters>();
    }

    private void OnEnable()
    {
        _inputReader.MainThrusterEvent += MainThrusterFired;
        _inputReader.ReverseThrusterEvent += ReverseThrusterFired;
        _inputReader.LeftThrustEvent += ThrustLeft;
        _inputReader.RightThrustEvent += ThrustRight;
        _inputReader.RotationThrustersEvent += RotationThrustersFired;
        _inputReader.StepLeftEvent += StepLeft;
        _inputReader.StepRightEvent += StepRight;
        _inputReader.BoostEvent += Boost;
        _inputReader.QuickTurnEvent += QuickTurn;
        _inputReader.BrakeEvent += Brake;
    }

    private void OnDisable()
    {
        _inputReader.MainThrusterEvent -= MainThrusterFired;
        _inputReader.ReverseThrusterEvent -= ReverseThrusterFired;
        _inputReader.LeftThrustEvent -= ThrustLeft;
        _inputReader.RightThrustEvent -= ThrustRight;
        _inputReader.RotationThrustersEvent -= RotationThrustersFired;
        _inputReader.StepLeftEvent -= StepLeft;
        _inputReader.StepRightEvent -= StepRight;
        _inputReader.BoostEvent -= Boost;
        _inputReader.QuickTurnEvent -= QuickTurn;
        _inputReader.BrakeEvent -= Brake;
    }

    private void FixedUpdate()
    {
        PerformMovement();

        HandleThrusters();

        if(_speedLimitEnabled)
            LimitVelocity();

        LimitRotation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FinishLine")
        {
            _OnCrossedFinishLine.RaiseEvent();
        }

        // TODO: trigger for out of bound areas?
    }

    private void PerformMovement()
    {
        if (_brake) 
        {
            // Brake in the opposite direction of our velocity
            Vector2 brakeForce = (_rigidbody2D.velocity * -1) * _brakeForce;
            _rigidbody2D.AddForce(brakeForce);
        } 
        else 
        {
            float finalMainThrustForce = _mainthrustForce;
            float finalMainInputValue = _mainThrusterInputValue;
            if (_boosting && _boostTank > 0f)
            {
                finalMainThrustForce *= _boostForceMultiplier;
                finalMainInputValue = 1f;
                _boostTank -= _boostCost * Time.deltaTime;
            }
            _rigidbody2D.AddForce(CalculateThrustForce(transform.up, finalMainThrustForce, finalMainInputValue));
            _rigidbody2D.AddForce(CalculateThrustForce(-transform.up, _reverseThrustForce, _reverseThrusterInputValue));
            _rigidbody2D.AddForce(CalculateThrustForce(transform.right, _horizontalThrustForce, _leftThrusterInputValue));
            _rigidbody2D.AddForce(CalculateThrustForce(-transform.right, _horizontalThrustForce, _rightThrusterInputValue));
        }

        _rigidbody2D.AddTorque(_rotForce * -_rotInputValue.x);
    }

    private Vector2 CalculateThrustForce(Vector2 dir, float thrusterForce, float inputValue)
    {
        // only apply if we are moving
        if (_rigidbody2D.velocity != Vector2.zero)
        {
            float angleBetween = Vector2.Angle(_rigidbody2D.velocity.normalized, dir.normalized);
            angleBetween = Mathf.Abs(angleBetween);
            if (angleBetween > _extremeDirChangeAngle)
            {
                float dirMult = angleBetween / 180;
                thrusterForce += thrusterForce * _extremeDirChangeMult * dirMult;
            }
        }

        return dir * thrusterForce * inputValue;
    }

    private void HandleThrusters() {
        if (_thrusters != null) {
            if (_brake)
            {
                _thrusters.BackThrusters(1.0f);
                _thrusters.LeftThrusters(1.0f);
                _thrusters.RightThrusters(1.0f);
                //_thrusters.NoseThrusters(1.0f);
            }
            else
            {
                _thrusters.BackThrusters(_mainThrusterInputValue);
                _thrusters.LeftThrusters(_leftThrusterInputValue);
                _thrusters.RightThrusters(_rightThrusterInputValue);
                _thrusters.NoseThrusters(_rotInputValue.x);
            }
        }
    }

    /// <summary>
    /// not the most super realistic, but might be ok?
    /// do not limit speed while stepping
    /// </summary>
    private void LimitVelocity()
    {
        float curMaxSpeed = _maxSpeed;
        if(_boosting)
        {
            if (_boostTank > 0)
            {
                curMaxSpeed *= _maxSpeedBoostModifier;
                ReduceBoost();
            }
            else
            {
                _boosting = false;
            }
        }

        if(_rigidbody2D.velocity.sqrMagnitude > curMaxSpeed * curMaxSpeed)
        { 
            Vector3 slowDownForce = _rigidbody2D.velocity.normalized * -_overSpeedLimitSlowdownForce;
            if (_boosting)
            {
                slowDownForce *= _boostForceMultiplier;
            }
            _rigidbody2D.AddForce(slowDownForce);
            if (_rigidbody2D.velocity.sqrMagnitude < curMaxSpeed * curMaxSpeed)
            {
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * curMaxSpeed;
            }
        }
    }

    private void LimitRotation()
    {
        if(Mathf.Abs(_rigidbody2D.angularVelocity) > _maxRotSpeed)
        {
            _rigidbody2D.angularVelocity = _maxRotSpeed * Mathf.Sign(_rigidbody2D.angularVelocity);
        }    
    }

    private void ReduceBoost()
    {
        if (_boosting)
        {
            _boostTank -= _boostCost * Time.deltaTime;
            _onBoostLevelChanged.RaiseEvent(_boostTank);
        }
    }

    private void PerformStepInitial()
    {
        var initialForce = _stepDir * _stepForce;
        _rigidbody2D.AddForce(initialForce);
        _speedLimitEnabled = false;
        StartCoroutine(FireCounterStepAfterTimer());
    }

    private IEnumerator FireCounterStepAfterTimer()
    {
        yield return new WaitForSeconds(_stepTime);
        _rigidbody2D.AddForce(-_stepDir * _stepForce / 2);
        _stepDir = Vector2.zero;
        StartCoroutine(EnableSpeedLimitAfterTimer());
    }

    private IEnumerator EnableSpeedLimitAfterTimer()
    {
        yield return new WaitForSeconds(_speedLimitEnableAfterStepTime);
        Debug.Log("speed limit enabled");
        _speedLimitEnabled = true;
    }

    public void RefillBoost(float fillSpeed) {
        if(_boostTank <= 100) {
            _boostTank += fillSpeed * Time.deltaTime;

            if(_boostParticles != null && !_boostParticles.isPlaying) {
                _boostParticles.Play();
            }
        } else {
            _boostTank = 100;
        }
        _onBoostLevelChanged.RaiseEvent(_boostTank);
    }

    #region InputEvents
    private void MainThrusterFired(float value)
    {
        _mainThrusterInputValue = value;
    }

    private void ReverseThrusterFired(float value)
    {
        _reverseThrusterInputValue = value;
    }

    private void ThrustLeft(float value)
    {
        _leftThrusterInputValue = value;
    }

    private void ThrustRight(float value)
    {
        _rightThrusterInputValue = value;
    }

    private void RotationThrustersFired(Vector2 value)
    {
        _rotInputValue = value;
    }

    private void StepLeft()
    {
        Debug.Log("stepLeft");
        if (_stepDir == Vector2.zero)
        {
            _stepDir = -transform.right;
            PerformStepInitial();
        }
    }

    private void StepRight()
    {
        Debug.Log("stepRight");
        if (_stepDir == Vector2.zero)
        {
            _stepDir = transform.right;
            PerformStepInitial();
        }
    }

    private void Boost(float value) {
        if(value > 0 && _boostTank > 0f) {
            _boosting = true;
        } else {
            _boosting = false;
        }
    }

    private void QuickTurn() {

    }

    private void Brake(float value) {
        if (value > 0) {
            // Brake
            _brake = true;
        } else {
            // Stop braking
            _brake = false;
        }
    }
    #endregion
}
