using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class ShipController : MonoBehaviour {

    public bool Boosting => _boosting;

    [Header("Thruster Properties")]
    [SerializeField] private float _mainthrustForce;
    [SerializeField] private float _reverseThrustForce;
    [SerializeField] private float _horizontalThrustForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSpeed;

    [Header("extreme direction change")]
    [SerializeField] private float _extremeDirChangeAngle;
    [SerializeField] private float _extremeDirChangeMult;

    [Header("Turning")]
    [SerializeField] private float _rotForce;
    [SerializeField] private float _maxRotSpeed;

    [Header("Boosting")]
    [SerializeField] private float _boostForceMultiplier;
    [SerializeField] private float _boostCost;
    [SerializeField] private float _maxSpeedBoostModifier;

    [Header("BoostPad")]
    [SerializeField] private float _boostPadNoSpeedLimitTime;

    [Header("Over Speed limit Slowdown")]
    [SerializeField] private float _overSpeedLimitSlowdownForce;

    [Header("Broadcasting On")]
    [SerializeField] private GameObjectEventChannelSO _OnCrossedFinishLine = default;
    [SerializeField] private FloatEventChannelSO _onHealthLevelChanged = default;
    [SerializeField] private FloatEventChannelSO _onBoostLevelChanged = default;
    [SerializeField] private GameObjectEventChannelSO _OnShipDestoryed = default;

    [Header("Effects")]
    [SerializeField] ParticleSystem _destructionParticles;

    [Header("Ship Stats")]
    [SerializeField] private float _health = 100.0f;
    [SerializeField] private float _boostTank = 0f;

    [SerializeField] private GameObject _shipModel;

    // accessors
    public float Health => _health;
    public float BoostTank { get => _boostTank; }
    public GameObject ShipModel => _shipModel;

    // required components
    private Rigidbody _rigidbody = default;
    private Animator _animator = default;
    private ShipThrusters _thrusters;

    // Input values
    private float _mainThrusterInputValue;
    private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;
    private bool _brake;
    private bool _boosting;

    // Boost pad
    private bool _boostPadActive;
    private int _boostPadActiveCount;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _thrusters = GetComponentInChildren<ShipThrusters>();
    }

    private void FixedUpdate()
    {
        if (_health > 0)
        {
            PerformMovement();

            HandleThrusters();

            LimitVelocity();

            LimitRotation();
        }
    }

    #region Collisions
    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "FinishLine") {
            _OnCrossedFinishLine.RaiseEvent(gameObject);
        }

        if(collision.tag == "Hazard") {
            ChangeHealth(-collision.GetComponent<Hazard>().damage);
        }
    }
    #endregion

    public void ChangeHealth(float amount)
    {
        if (_health > 0)
        {
            _health += amount;
            if (_health < 0)
            {
                // TODO: Explode
                _destructionParticles.Play();
                _OnShipDestoryed.RaiseEvent(gameObject);
                _shipModel.SetActive(false);
                _rigidbody.velocity = Vector3.zero;

                _health = 0;
            }

            if (_health > 100)
            {
                _health = 100;
            }

            if (_onHealthLevelChanged != null)
            {
                _onHealthLevelChanged.RaiseEvent(_health);
            }
        }
    }

    public void RefillBoost(float fillSpeed)
    {
        if (_boostTank <= 100)
        {
            _boostTank += fillSpeed;
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

    public void BoostPadActivate(float boostPadForce)
    {
        _boostPadActive = true;
        _boostPadActiveCount++;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(transform.forward * boostPadForce * _mainthrustForce);

        StartCoroutine(DisableBoostPadBoost());
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
                ReduceBoost(_boostCost * Time.deltaTime);
            } else if (_boosting && BoostTank <= 0f)
            {
                _boosting = false;
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
        float curSlowdown = _overSpeedLimitSlowdownForce;
        if (_boostPadActive || _boosting)
        {
            curMaxSpeed *= _maxSpeedBoostModifier;
            curSlowdown *= _boostForceMultiplier;
        }

        if(_rigidbody.velocity.sqrMagnitude > curMaxSpeed * curMaxSpeed)
        {
            // slow down at a higher rate when we are further over the speed limit
            float curSlowdownRate = _rigidbody.velocity.sqrMagnitude / (curMaxSpeed * curMaxSpeed) * curSlowdown;
            Vector3 slowDownForce = _rigidbody.velocity.normalized * -curSlowdownRate;

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
        if(_onBoostLevelChanged)
        {
            _onBoostLevelChanged.RaiseEvent(_boostTank);
        }
    }

    private IEnumerator DisableBoostPadBoost()
    {
        yield return new WaitForSeconds(_boostPadNoSpeedLimitTime);
        _boostPadActiveCount--;
        if (_boostPadActiveCount <= 0)
        {
            _boostPadActiveCount = 0;
            _boostPadActive = false;
        }
    }
}
