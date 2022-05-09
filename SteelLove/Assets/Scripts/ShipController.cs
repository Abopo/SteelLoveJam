using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour {

    [SerializeField] private float _mainthrustForce;
    [SerializeField] private float _reverseThrustForce;
    [SerializeField] private float _horizontalThrustForce;
    [SerializeField] private float _stepForce;
    [SerializeField] private float _brakeForce;

    [SerializeField] private float _rotForce;

    [SerializeField] private float _stepTime;
    [SerializeField] private float _speedLimitEnableAfterStepTime;

    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxRotSpeed;

    [SerializeField] private InputReader _inputReader = default;

    private Rigidbody2D _rigidbody2D = default;

    private float _mainThrusterInputValue;
    private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;
    private bool _brake;

    private Vector2 _stepDir;
    private bool _speedLimitEnabled = true;

    ShipThrusters _thrusters;

    private float _boostTank = 0f;
    public float BoostTank { get => _boostTank; }
    [SerializeField] ParticleSystem _boostParticles;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _thrusters = GetComponentInChildren<ShipThrusters>();

        //TODO: move to a gameplayState manager
        _inputReader.EnableRacingInput();
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

    private void PerformMovement()
    {
        if (_brake) 
        {
            // Brake in the opposite direction of our velocity
            Vector2 brakeForce = (_rigidbody2D.velocity * -1) * _brakeForce;
            _rigidbody2D.AddForce(brakeForce);

            // Not sure if it feels right to stop player from rotating while braking
            /*
            if (_rigidbody2D.angularVelocity != 0) {
                _rigidbody2D.AddTorque(-Mathf.Sign(_rigidbody2D.angularVelocity) * _rotForce);
            }
            */
        } 
        else 
        {
            Vector2 addingForce = transform.up * _mainthrustForce * _mainThrusterInputValue;
            _rigidbody2D.AddForce(addingForce);

            addingForce = -transform.up * _reverseThrustForce * _reverseThrusterInputValue;
            _rigidbody2D.AddForce(addingForce);

            addingForce = transform.right * _horizontalThrustForce * _leftThrusterInputValue;
            _rigidbody2D.AddForce(addingForce);

            addingForce = -transform.right * _horizontalThrustForce * _rightThrusterInputValue;
            _rigidbody2D.AddForce(addingForce);
        }

        _rigidbody2D.AddTorque(_rotForce * -_rotInputValue.x);
    }

    private void HandleThrusters() {
        _thrusters.BackThrusters(_mainThrusterInputValue);
        _thrusters.LeftThrusters(_leftThrusterInputValue);
        _thrusters.RightThrusters(_rightThrusterInputValue);
        _thrusters.NoseThrusters(_rotInputValue.x);
    }

    /// <summary>
    /// not the most super realistic, but might be ok?
    /// do not limit speed while stepping
    /// </summary>
    private void LimitVelocity()
    {
        if(_rigidbody2D.velocity.sqrMagnitude > _maxSpeed * _maxSpeed)
        {
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;
        }
    }

    private void LimitRotation()
    {
        if(Mathf.Abs(_rigidbody2D.angularVelocity) > _maxRotSpeed)
        {
            _rigidbody2D.angularVelocity = _maxRotSpeed * Mathf.Sign(_rigidbody2D.angularVelocity);
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
        if(value > 0) {
            // Activate boost

        } else {
            // Stop boosting

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
