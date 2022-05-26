using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class ShipController : MonoBehaviour {

    public bool Boosting => _boosting;

    [Header("Movement Properties")]
    [SerializeField] private float _mainthrustForce;
    [SerializeField] private float _boostForceMultiplier;
    [SerializeField] private float _boostPadForceMultiplier;
    [SerializeField] private float _reverseThrustForce;
    [SerializeField] private float _horizontalThrustForce;
    [SerializeField] private float _stepForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _extremeDirChangeAngle;
    [SerializeField] private float _extremeDirChangeMult;

    [SerializeField] private float _rotForce;

    [SerializeField] private float _stepTime;
    [SerializeField] private float _speedLimitEnableAfterStepTime;
    [SerializeField] private float _stepCooldown;

    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxSpeedBoostModifier;
    [SerializeField] private float _overSpeedLimitSlowdownForce;
    [SerializeField] private float _maxRotSpeed;

    [SerializeField] private float _stepCost;
    [SerializeField] private float _boostCost;
    [SerializeField] private float _boostPadTime;

    [Header("Input")]
    [SerializeField] private InputReader _inputReader = default;

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _OnCrossedFinishLine = default;
    [SerializeField] private FloatEventChannelSO _onHealthLevelChanged = default;
    [SerializeField] private FloatEventChannelSO _onBoostLevelChanged = default;

    [Header("Effects")]
    [SerializeField] ParticleSystem _boostParticles;

    [Header("Ship Stats")]
    // Ship Resources
    [SerializeField] private float _health = 100.0f;
    [SerializeField] private float _boostTank = 0f;

    public float Health => _health;
    
    private bool _isOutsideOfTrack;

    private Rigidbody _rigidbody = default;
    private Animator _animator = default;

    private float _mainThrusterInputValue;
    private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;
    private bool _brake;
    private bool _boosting;
    private bool _boostPadActive;

    private Vector2 _stepDir;
    private bool _canStep = true;
    private bool _speedLimitEnabled = true;

    ShipThrusters _thrusters;

    public float BoostTank { get => _boostTank; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _thrusters = GetComponentInChildren<ShipThrusters>();
    }

    private void FixedUpdate()
    {
        PerformMovement();

        HandleThrusters();

        if(_speedLimitEnabled)
            LimitVelocity();

        LimitRotation();
    }

    #region Collisions

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "FinishLine") {
            _OnCrossedFinishLine.RaiseEvent();
        }

        // TODO: trigger for out of bound areas?
        if (collision.tag == "InsideTrack") {
            _isOutsideOfTrack = false;
        }

        if(collision.tag == "Hazard") {
            ChangeHealth(-collision.GetComponent<Hazard>().damage);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "InsideTrack") {
            _isOutsideOfTrack = true;
        }
    }

    #endregion

    public void ChangeHealth(float amount)
    {
        _health += amount;
        if (_health < 0)
        {
            // TODO: Explode

            _health = 0;
        }

        if (_health > 100)
        {
            _health = 100;
        }

        if (_onHealthLevelChanged != null) {
            _onHealthLevelChanged.RaiseEvent(_health);
        }
    }

    public void RefillBoost(float fillSpeed)
    {
        if (_boostTank <= 100)
        {
            _boostTank += fillSpeed;

            if (_boostParticles != null && !_boostParticles.isPlaying)
            {
                _boostParticles.Play();
            }
        }
        else
        {
            _boostTank = 100;
        }

        if (_onBoostLevelChanged != null) {
            _onBoostLevelChanged.RaiseEvent(_boostTank);
        }
    }

    public void ThrustForward(float value)
    {
        _mainThrusterInputValue = value;
        _reverseThrusterInputValue = 0;
    }

    public void ThrustBackwards(float value)
    {
        _reverseThrusterInputValue = value;
        _mainThrusterInputValue = 0;
    }

    public void ThrustLeft(float value)
    {
        _leftThrusterInputValue = value;
        _rightThrusterInputValue = 0;
    }

    public void ThrustRight(float value)
    {
        _rightThrusterInputValue = value;
        _leftThrusterInputValue = 0;
    }

    public void RotationThrust(Vector2 value)
    {
        _rotInputValue = value;
    }

    public void StepLeft()
    {
        AttemptStepInitial(-transform.right);
    }

    public void StepRight()
    {
        AttemptStepInitial(transform.right);
    }

    public void Boost(float value)
    {
        if (value > 0 && _boostTank > 0f)
        {
            _boosting = true;
        }
        else
        {
            _boosting = false;
        }
    }

    public void Brake(float value)
    {
        if (value > 0)
        {
            // Brake
            _brake = true;
        }
        else
        {
            // Stop braking
            _brake = false;
        }
    }

    public void BoostPadActivate()
    {
        _boostPadActive = true;
    }

    private void PerformMovement()
    {
        if (_brake) 
        {
            // Brake in the opposite direction of our velocity
            Vector3 brakeForce = (_rigidbody.velocity * -1) * _brakeForce;
            _rigidbody.AddForce(brakeForce);
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

            _rigidbody.AddForce(CalculateThrustForce(transform.forward, finalMainThrustForce, finalMainInputValue));
            _rigidbody.AddForce(CalculateThrustForce(-transform.forward, _reverseThrustForce, _reverseThrusterInputValue));
            _rigidbody.AddForce(CalculateThrustForce(transform.right, _horizontalThrustForce, _leftThrusterInputValue));
            _rigidbody.AddForce(CalculateThrustForce(-transform.right, _horizontalThrustForce, _rightThrusterInputValue));

            // set animatior
            _animator.SetFloat("LeftBoostInput", _leftThrusterInputValue);
            _animator.SetFloat("RightBoostInput", _rightThrusterInputValue);
        }
        _rigidbody.AddTorque(-transform.up * _rotForce * -_rotInputValue.x);
    }

    private Vector3 CalculateThrustForce(Vector3 dir, float thrusterForce, float inputValue)
    {
        // only apply if we are moving
        if (_rigidbody.velocity != Vector3.zero)
        {
            float angleBetween = Vector3.Angle(_rigidbody.velocity.normalized, dir.normalized);
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
                _thrusters.NoseThrusters(_reverseThrustForce);
                _thrusters.LeftThrusters(_leftThrusterInputValue);
                _thrusters.RightThrusters(_rightThrusterInputValue);
                _thrusters.TurnThrusters(_rotInputValue.x);
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
                ReduceBoost(_boostCost * Time.deltaTime);
            }
            else
            {
                _boosting = false;
            }
        }

        if(_rigidbody.velocity.sqrMagnitude > curMaxSpeed * curMaxSpeed)
        { 
            Vector3 slowDownForce = _rigidbody.velocity.normalized * -_overSpeedLimitSlowdownForce;
            if (_boosting)
            {
                slowDownForce *= _boostForceMultiplier;
            }
            _rigidbody.AddForce(slowDownForce);
            if (_rigidbody.velocity.sqrMagnitude < curMaxSpeed * curMaxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * curMaxSpeed;
            }
        }
    }

    private void LimitRotation()
    {
        if(Mathf.Abs(_rigidbody.angularVelocity.magnitude) > _maxRotSpeed)
        {
            _rigidbody.angularVelocity = _maxRotSpeed * _rigidbody.angularVelocity.normalized;
        }    
    }

    private void ReduceBoost(float cost)
    {
        _boostTank -= cost;
        _onBoostLevelChanged.RaiseEvent(_boostTank);
    }

    private void AttemptStepInitial(Vector2 stepDir)
    {
        if (_stepDir == Vector2.zero && _canStep == true && _boostTank >= _boostCost)
        {
            _stepDir = stepDir;
            ReduceBoost(_stepCost);
            var initialForce = _stepDir * _stepForce;
            _rigidbody.AddForce(initialForce);
            _canStep = false;
            _speedLimitEnabled = false;
            StartCoroutine(FireCounterStepAfterTimer());
            StartCoroutine(ReEnableStepAfterTimer());
        }
    }

    private IEnumerator FireCounterStepAfterTimer()
    {
        yield return new WaitForSeconds(_stepTime);
        _rigidbody.AddForce(-_stepDir * _stepForce * .8f);
        _stepDir = Vector3.zero;
        StartCoroutine(EnableSpeedLimitAfterTimer());
    }

    private IEnumerator ReEnableStepAfterTimer()
    {
        yield return new WaitForSeconds(_stepCooldown);
        _canStep = true;
    }

    private IEnumerator EnableSpeedLimitAfterTimer()
    {
        yield return new WaitForSeconds(_speedLimitEnableAfterStepTime);
        _speedLimitEnabled = true;
    }
}
