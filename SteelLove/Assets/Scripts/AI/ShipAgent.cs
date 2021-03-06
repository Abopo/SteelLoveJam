using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ShipAgent : Agent {

    Rigidbody _rBody;
    ShipController _ship;

    [SerializeField] AICheckpoint _firstCheckpoint;
    [SerializeField] AICheckpoint _nextCheckpoint;
    float _lastCheckpointTime = 0f;

    [SerializeField] float _curHealth = 100f;

    // Input
    Vector2 _leftControlSignal;
    Vector2 _rightControlSignal;

    [SerializeField] private InputReader _inputReader = default;
    private float _mainThrusterInputValue;
    private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;

    TrackDamage _trackDamage;

    AgentManager _agentManager;

    // Start is called before the first frame update
    void Start() {
        _rBody = GetComponent<Rigidbody>();
        _ship = GetComponent<ShipController>();
        _trackDamage = GetComponent<TrackDamage>();
        _agentManager = GetComponentInParent<AgentManager>();

        Reset();
    }
    private void Reset() {
        // Set to start of the track (or a random position on the track?)
        _ship.transform.position = new Vector3(0f, 0.5f, 0f);

        _rBody.velocity = Vector3.zero;

        //ResetAllCheckpoints();
        //_firstCheckpoint = _agentManager.GetRandomCheckpoint();
        _firstCheckpoint = _agentManager.FirstCheckpoint();
        transform.position = new Vector3(_firstCheckpoint.center.x, 0.5f, _firstCheckpoint.center.z);
        _nextCheckpoint = _firstCheckpoint.nextCheckpoint;

        // Reset health
        _ship.ChangeHealth(100);
        _curHealth = 100;
    }

    protected override void OnEnable() {
        base.OnEnable();

        _inputReader.MainThrusterEvent += GetThrustForward;
        _inputReader.ReverseThrusterEvent += GetThrustBackwards;
        _inputReader.LeftThrustEvent += GetThrustLeft;
        _inputReader.RightThrustEvent += GetThrustRight;
        _inputReader.RotationThrustersEvent += GetRotationThrust;
    }

    protected override void OnDisable() {
        base.OnDisable();

        _inputReader.MainThrusterEvent -= GetThrustForward;
        _inputReader.ReverseThrusterEvent -= GetThrustBackwards;
        _inputReader.LeftThrustEvent -= GetThrustLeft;
        _inputReader.RightThrustEvent -= GetThrustRight;
        _inputReader.RotationThrustersEvent -= GetRotationThrust;
    }

    /*
    This is the ship agent for ML AI stuff
    Process:

    
    OnEpisodeBegin:
    
    On CollectObservations(VectorSensor sensor):
    Watch the track with raycasts that collide with the TrackWallsAI layer
    
    OnActionReceived:
    Feed input into the ship controller via X and Y coordinates simulating a joystick
    Input for both left and right joystick for velocity and rotation respectively

    Rewards:
    Reward based on current speed every step (small)
    Reaching a checkpoint (big)
    Face the direction of the reached checkpoint (small)

    Punishments:
    Small loss every step (encourages moving faster)
    Losing health

    Reset:
    Finish a lap
    Run out of health
    */

    public override void OnEpisodeBegin() {
        base.OnEpisodeBegin();

        if (_curHealth <= 0) {
            Reset();
        }
    }


    void ResetAllCheckpoints() {
        AICheckpoint tempCheckpoint = _firstCheckpoint;
        while (tempCheckpoint.nextCheckpoint != null) {
            tempCheckpoint.nextCheckpoint.gameObject.SetActive(false);
            tempCheckpoint = tempCheckpoint.nextCheckpoint;
        }

        _firstCheckpoint.gameObject.SetActive(true);
    }

    public override void CollectObservations(VectorSensor sensor) {
        if (_nextCheckpoint != null) {

            //sensor.AddObservation(_nextCheckpoint.transform.position);
            sensor.AddObservation(_nextCheckpoint.center);

            sensor.AddObservation(transform.position);

            sensor.AddObservation(_rBody.velocity);

            sensor.AddObservation(_trackDamage.IsOffTrack());
            //float distToCheckpoint = Vector3.Distance(transform.position, _nextCheckpoint.position);
            //sensor.AddObservation(distToCheckpoint);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // Actions, size 3

        // Left stick input
        _leftControlSignal = Vector2.zero;
        _leftControlSignal.x = actions.ContinuousActions[0];
        _leftControlSignal.y = actions.ContinuousActions[1];
        Mathf.Clamp(_leftControlSignal.x, -1f, 1f);
        Mathf.Clamp(_leftControlSignal.y, -1f, 1f);

        // Right stick input
        _rightControlSignal.x = actions.ContinuousActions[2];
        Mathf.Clamp(_rightControlSignal.x, -1f, 1f);

        // Send input data to ShipController
        SendInputToShip();

        // Rewards
        GiveRewards();
    }

    void SendInputToShip() {
        if (_leftControlSignal.y > 0) {
            _ship.ThrustForward(_leftControlSignal.y);
        } else {
            _ship.ThrustBackwards(Mathf.Abs(_leftControlSignal.y));
        }

        if (_leftControlSignal.x > 0) {
            _ship.ThrustRight(_leftControlSignal.x);
        } else {
            _ship.ThrustLeft(Mathf.Abs(_leftControlSignal.x));
        }

        //_ship.RotationThrust(_rightControlSignal);
    }

    void GiveRewards() {
        // Small punishment every tick to encourage moving to checkpoints quickly
        AddReward(-0.0001f);

        // Speed
        //AddReward(_rBody.velocity.magnitude * 0.0002f);

        // Lap

        // Health
        float healthDif = _curHealth - _ship.Health;
        // If the ship had lost some health
        if (healthDif > 0) {
            // Punishment
            AddReward(-0.01f);
            _curHealth = _ship.Health;

            if (_curHealth <= 0) {
                EndEpisode();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "CheckpointAI") {
            OnCheckpointPassed(other.GetComponent<AICheckpoint>());
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "TrackWallsAI") {
            /*
            // Insta wall death
            _curHealth = 0f;
            SetReward(-1);
            EndEpisode();
            */

            
            AddReward(_rBody.velocity.magnitude * -0.005f);
            
            _curHealth -= 10f;
            if (_curHealth <= 0) {
                EndEpisode();
            }
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.collider.tag == "TrackWallsAI") {
            AddReward(-0.001f);

            _curHealth -= 0.1f;
            if (_curHealth <= 0) {
                SetReward(-1);
                EndEpisode();
            }
        }
    }

    public void OnCheckpointPassed(AICheckpoint checkpoint) {
        if (checkpoint == _nextCheckpoint) {
            SetReward(1);
            _nextCheckpoint = checkpoint.nextCheckpoint;
        } else {
            // Only reward if hit checkpoint has higher id and is somewhat close.
            if (checkpoint.id > _nextCheckpoint.id && checkpoint.id - _nextCheckpoint.id < 10) {
                SetReward(0.5f);
                _nextCheckpoint = checkpoint.nextCheckpoint;
            } else {
                AddReward(-0.5f);
            }
        }

        // TODO: actually just loop them forever
        if (_nextCheckpoint == null) {
            // We've hit the last checkpoint, so reset
            Debug.Log("Hit final checkpoint!");
            _curHealth = 0f;
            SetReward(1);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;

        if(_mainThrusterInputValue > 0) {
            continuousActionsOut[1] = _mainThrusterInputValue;
        } else {
            continuousActionsOut[1] = -_reverseThrusterInputValue;
        }

        if(_rightThrusterInputValue > 0) {
            continuousActionsOut[0] = _rightThrusterInputValue;
        } else {
            continuousActionsOut[0] = -_leftThrusterInputValue;
        }

        //continuousActionsOut[2] = _rotInputValue.x;
    }

    public void GetThrustForward(float value) {
        _mainThrusterInputValue = value;
        _reverseThrusterInputValue = 0;
    }

    public void GetThrustBackwards(float value) {
        _reverseThrusterInputValue = value;
        _mainThrusterInputValue = 0;
    }

    public void GetThrustLeft(float value) {
        _leftThrusterInputValue = value;
        _rightThrusterInputValue = 0;
    }

    public void GetThrustRight(float value) {
        _rightThrusterInputValue = value;
        _leftThrusterInputValue = 0;
    }

    public void GetRotationThrust(Vector2 value) {
        _rotInputValue = value;
    }
}
