using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ShipAgent : Agent {

    [SerializeField] private FloatEventChannelSO _onHealthChanged;

    Rigidbody _rBody;
    ShipController _ship;

    Transform _closestCheckpoint;

    float _curHealth = 100f;

    // Input
    Vector2 _leftControlSignal;
    Vector2 _rightControlSignal;

    // Start is called before the first frame update
    void Start() {
        _rBody = GetComponentInParent<Rigidbody>();
        _ship = GetComponentInParent<ShipController>();

    }

    protected override void OnEnable() {
        base.OnEnable();
        _onHealthChanged.OnEventRaised += OnHealthChanged;
    }

    protected override void OnDisable() {
        base.OnDisable();
        _onHealthChanged.OnEventRaised -= OnHealthChanged;
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

        // Set to start of the track (or a random position on the track?)
        transform.position = Vector3.zero;
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // Actions, size 3

        // Left stick input
        Vector2 leftControlSignal = Vector2.zero;
        leftControlSignal.x = actions.ContinuousActions[0];
        leftControlSignal.y = actions.ContinuousActions[1];
        Mathf.Clamp(leftControlSignal.x, -1f, 1f);
        Mathf.Clamp(leftControlSignal.y, -1f, 1f);

        // Right stick input
        _rightControlSignal.x = actions.ContinuousActions[2];
        Mathf.Clamp(_rightControlSignal.x, -1f, 1f);

        // Send input data to ShipController
        SendInputToShip();

        // Find closest checkpoint

        // Rewards
        GiveRewards();
    }

    void SendInputToShip() {
        if (_leftControlSignal.y >= 0) {
            _ship.ThrustForward(_leftControlSignal.y);
        } else {
            _ship.ThrustBackwards(_leftControlSignal.y);
        }

        if(_leftControlSignal.x >= 0) {
            _ship.ThrustRight(_leftControlSignal.x);
        } else {
            _ship.ThrustLeft(_leftControlSignal.x);
        }

        _ship.RotationThrust(_rightControlSignal);
    }

    void GiveRewards() {
        /*
         * First let's start extremely simple, just speed and health
         * then, increase complexity later.
         */

        // Speed
        AddReward(_rBody.velocity.magnitude * 0.001f);

        // Checkpoints

        // Lap

        // Health
        // v See below v
    }
    
    void OnHealthChanged(float health) {
        // Punish based on amount of health lost
        float healthDif = _curHealth - health;
        AddReward(healthDif * -0.01f);
        _curHealth = health;

        if(_curHealth <= 0) {
            EndEpisode();
        }
    }

    public void OnCheckpointPassed() {
        // Give reward

        // Set next checkpoint

    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;

    }
}
