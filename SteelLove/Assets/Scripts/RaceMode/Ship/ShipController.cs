using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class ShipController : MonoBehaviour {

    public bool Boosting => _boosting;

    [SerializeField] CharacterSO m_Character;
    [SerializeField] private SceneManagerSO _sceneManager;
    [SerializeField] private GameSceneSO _breakRoomScene;

    [Header("Broadcasting On")]
    [SerializeField] private GameObjectEventChannelSO _OnCrossedFinishLine = default;
    [SerializeField] private FloatEventChannelSO _onHealthLevelChanged = default;
    [SerializeField] private FloatEventChannelSO _onBoostLevelChanged = default;
    [SerializeField] private GameObjectEventChannelSO _OnShipDestoryed = default;

    [Header("Listening To")]
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;

    [Header("Effects")]
    [SerializeField] ParticleSystem _destructionParticles;
    [SerializeField] ParticleSystem _shipFireParticles;

    [Header("Ship Stats")]
    [SerializeField] private ShipStatsSO _shipStats;
    private float _maxHealth;
    [SerializeField] private float _healthGainOnCheckpoint;
    [SerializeField] private float _healthLossOnGeneralCollision;

    private float _modifiedMaxSpeed;
    private float _modifiedHorizontalThrustForce;
    private float _curHealth;
    private float _curBoostTank;

    [SerializeField] private GameObject _shipModel;

    public Transform spawnPoint;

    // accessors
    public CharacterSO Character => m_Character;
    public float Health => _curHealth;
    public float MaxHealth => _maxHealth;
    public float BoostTank { get => _curBoostTank; }
    public GameObject ShipModel => _shipModel;

    // required components
    private Rigidbody _rigidbody = default;
    private Animator _animator = default;
    private ShipThrusters _thrusters;

    // Input values
    private float _mainThrusterInputValue;
    [SerializeField] private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;
    private bool _brake;
    private bool _boosting;

    // Boost pad
    private bool _boostPadActive;
    private int _boostPadActiveCount;

    ShipAudio _shipAudio;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _thrusters = GetComponentInChildren<ShipThrusters>();

        _shipAudio = GetComponentInChildren<ShipAudio>();
    }

    private void Start() {
        InitUpgradesAndSabatoge();

        _maxHealth = _shipStats._startingHealth;
        _curHealth = _shipStats._startingHealth;

        _curBoostTank = _shipStats._startingBoostTank;
    }

    private void OnEnable()
    {
        _onCrossedNextCheckpoint.OnEventRaised += CheckpointHeal;
    }

    private void OnDisable()
    {
        _onCrossedNextCheckpoint.OnEventRaised -= CheckpointHeal;
    }

    private void InitUpgradesAndSabatoge() {    
        // TEMP: removing for balance testing
        //var upgrades = m_Character.upgrades;
        //var sabatoged = m_Character.sabotaged;
        //// Not the best way, but the easiest way
        //if (_sceneManager.previousScene != _breakRoomScene) {
        //    upgrades = 5;
        //    sabatoged = false;
        //}

        //if (upgrades > 0) {
        //    // Handling increase
        //    _rotForce = 0.7f;
        //    _maxRotSpeed = 2f;
        //}
        //if (upgrades > 1) {
        //    // Acceleration increase
        //    _mainthrustForce = 17;
        //    _reverseThrustForce = 12;
        //    _horizontalThrustForce = 10;
        //}
        //if (upgrades > 2) {
        //    // Health increase
        //    _health = 125;
        //}
        //if (upgrades > 3) {
        //    // Boost increase
        //    _boostForceMultiplier = 4;
        //    _maxSpeedBoostModifier = 1.75f;
        //}
        //if (upgrades > 4) {
        //    // Top speed increase
        //    _maxSpeed = 28;
        //}
        //if (upgrades > 5) {
        //    // Overall increase
        //    _rotForce = 0.8f;
        //    _maxRotSpeed = 2.25f;
        //    _mainthrustForce = 19;
        //    _reverseThrustForce = 14;
        //    _horizontalThrustForce = 12;
        //    _health = 150;
        //    _boostForceMultiplier = 5;
        //    _maxSpeedBoostModifier = 1.9f;
        //    _maxSpeed = 30;
        //}

        //if (sabatoged) {
        //    // Lower health
        //    _health = 50;
        //}
    }

    private void FixedUpdate()
    {
        if (_curHealth > 0)
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
            if(collision.transform.parent.GetComponent<Ship_Engine_Hazard>() && _shipFireParticles != null)
            {
                Debug.Log("Start fire");
                _shipFireParticles.Play();
            }
            Debug.Log("health: " + _curHealth + " damage: " + collision.GetComponent<Hazard>().damage);
            ChangeHealth(-collision.GetComponent<Hazard>().damage);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Hazard") {
            ChangeHealth(-collision.gameObject.GetComponent<Hazard>().damage);
        }
        else
        {
            ChangeHealth(-_healthLossOnGeneralCollision);
        }
    }
    #endregion

    public void ChangeHealth(float amount)
    {
        if (_shipModel.activeInHierarchy)
        {
            _curHealth += amount;
            if (_curHealth < 0)
            {
                _destructionParticles.Play();
                _shipModel.SetActive(false);
                _rigidbody.velocity = Vector3.zero;
                
                if (_shipAudio != null) {
                    _shipAudio.PlayExplosion();
                }

                _curHealth = 0;

                // If easy mode, just respawn
                if (GameManager.instance != null && GameManager.instance.easyMode) {
                    StartCoroutine(Respawn());
                } else {
                    if(_shipAudio != null) {
                        _shipAudio.PlayDeathClip();
                    }
                    _OnShipDestoryed.RaiseEvent(gameObject);
                }
            }

            if (_curHealth > _maxHealth)
            {
                _curHealth = _maxHealth;
            }

            if (_onHealthLevelChanged != null)
            {
                _onHealthLevelChanged.RaiseEvent(_curHealth);
            }
        }
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(2.0f);

        // Respawn at last checkpoint
        _shipModel.SetActive(true);
        _destructionParticles.Stop();
        var lastCheckpoint = GetComponent<CheckpointTracker>().LastPassedCheckpoint;
        if (lastCheckpoint != null)
        {
            transform.position = GetComponent<CheckpointTracker>().LastPassedCheckpoint.transform.position;
        }
        else
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        ChangeHealth(_maxHealth);
    }

    public void RefillBoost(float fillSpeed)
    {
        if (_curBoostTank <= 100)
        {
            _curBoostTank += fillSpeed;
        }
        else
        {
            _curBoostTank = 100;
        }

        if (_onBoostLevelChanged != null) {
            _onBoostLevelChanged.RaiseEvent(_curBoostTank);
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
        if (value > 0 && _curBoostTank > 0f)
        {
            _boosting = true;
        }
        else
        {
            _boosting = false;
        }
    }

    public void BoostPadActivate(float boostPadForce)
    {
        _boostPadActive = true;
        _boostPadActiveCount++;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(transform.forward * boostPadForce * _shipStats._mainthrustForce);

        StartCoroutine(DisableBoostPadBoost());
    }

    private void PerformMovement()
    {
        float finalMainThrustForce = _shipStats._mainthrustForce;
        float finalMainInputValue = _mainThrusterInputValue;
        if (_boosting && _curBoostTank > 0f)
        {
            finalMainThrustForce *= _shipStats._boostForceMultiplier;
            finalMainInputValue = 1f;
            ReduceBoost(_shipStats._boostCost * Time.deltaTime);
        } else if (_boosting && BoostTank <= 0f)
        {
            _boosting = false;
        }

        _rigidbody.AddForce(CalculateThrustForce(transform.forward, finalMainThrustForce, finalMainInputValue));
        _rigidbody.AddForce(CalculateThrustForce(-transform.forward, _shipStats._reverseThrustForce, _reverseThrusterInputValue));
        _rigidbody.AddForce(CalculateThrustForce(transform.right, _shipStats._horizontalThrustForce, _leftThrusterInputValue));
        _rigidbody.AddForce(CalculateThrustForce(-transform.right, _shipStats._horizontalThrustForce, _rightThrusterInputValue));

        // set animatior
        _animator.SetFloat("LeftBoostInput", _leftThrusterInputValue);
        _animator.SetFloat("RightBoostInput", _rightThrusterInputValue);

        _rigidbody.AddTorque(-transform.up * _shipStats._rotForce * -_rotInputValue.x);
    }

    private Vector3 CalculateThrustForce(Vector3 dir, float thrusterForce, float inputValue)
    {
        // only apply if we are moving
        if (_rigidbody.velocity != Vector3.zero)
        {
            float angleBetween = Vector3.Angle(_rigidbody.velocity.normalized, dir.normalized);
            angleBetween = Mathf.Abs(angleBetween);
            if (angleBetween > _shipStats._extremeDirChangeAngle)
            {
                float dirMult = angleBetween / 180;
                thrusterForce += thrusterForce * _shipStats._extremeDirChangeMult * dirMult;
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
                _thrusters.NoseThrusters(_reverseThrusterInputValue);
                _thrusters.LeftThrusters(_leftThrusterInputValue);
                _thrusters.RightThrusters(_rightThrusterInputValue);
                _thrusters.TurnThrusters(_rotInputValue.x);

                _thrusters.BoostThruster(_boosting);
            }
        }
    }

    /// <summary>
    /// not the most super realistic, but might be ok?
    /// do not limit speed while stepping
    /// </summary>
    private void LimitVelocity()
    {
        float curMaxSpeed = _shipStats._maxSpeed;
        float curSlowdown = _shipStats._overSpeedLimitSlowdownForce;
        if (_boostPadActive || _boosting)
        {
            curMaxSpeed *= _shipStats._maxSpeedBoostModifier;
            curSlowdown *= _shipStats._boostForceMultiplier;
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
        if(Mathf.Abs(_rigidbody.angularVelocity.magnitude) > _shipStats._maxRotSpeed)
        {
            _rigidbody.angularVelocity = _shipStats._maxRotSpeed * _rigidbody.angularVelocity.normalized;
        }    
    }

    private void ReduceBoost(float cost)
    {
        _curBoostTank -= cost;
        if(_onBoostLevelChanged)
        {
            _onBoostLevelChanged.RaiseEvent(_curBoostTank);
        }
    }

    private IEnumerator DisableBoostPadBoost()
    {
        yield return new WaitForSeconds(_shipStats._boostPadNoSpeedLimitTime);
        _boostPadActiveCount--;
        if (_boostPadActiveCount <= 0)
        {
            _boostPadActiveCount = 0;
            _boostPadActive = false;
        }
    }

    private void CheckpointHeal(GameObject shipObj)
    {
        if (shipObj == gameObject)
        {
            ChangeHealth(_healthGainOnCheckpoint);
        }
    }

    public void AdjustShipParametersAI(float mainThrustMult, float horThrustMult, float maxSpeedMult) {
        //_mainThrusterInputValue *= mainThrustMult; // i dont think this does anything?
        _modifiedHorizontalThrustForce *= horThrustMult;
        _modifiedMaxSpeed *= maxSpeedMult;
    }
}
