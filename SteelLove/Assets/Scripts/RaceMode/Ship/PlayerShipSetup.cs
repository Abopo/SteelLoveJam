using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// seperates the input from ship controller so that AI can use the same ship controller.
/// for AI create a script that hooks into the public ship controller input functions
/// </summary>
[RequireComponent(typeof(ShipController))]
public class PlayerShipSetup : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    // used to help setup the scene when player spawns
    [SerializeField] private GameObject _cameraConstraint;
    [SerializeField] private GameObject _cameraLookAt;

    private ShipController _shipController;

    private CameraController _cameraController;
    private Speedometer _speedometer;

    private void Awake()
    {
        _shipController = GetComponent<ShipController>();

        _cameraController = FindObjectOfType<CameraController>();
        _speedometer = FindObjectOfType<Speedometer>();
    }

    private void OnEnable()
    {
        _inputReader.MainThrusterEvent += _shipController.ThrustForward;
        _inputReader.ReverseThrusterEvent += _shipController.ThrustBackwards;
        _inputReader.LeftThrustEvent += _shipController.ThrustLeft;
        _inputReader.RightThrustEvent += _shipController.ThrustRight;
        _inputReader.RotationThrustersEvent += _shipController.RotationThrust;
        _inputReader.StepLeftEvent += _shipController.StepLeft;
        _inputReader.StepRightEvent += _shipController.StepRight;
        _inputReader.BoostEvent += _shipController.Boost;
        _inputReader.BrakeEvent += _shipController.Brake;
    }

    private void OnDisable()
    {
        _inputReader.MainThrusterEvent -= _shipController.ThrustForward;
        _inputReader.ReverseThrusterEvent -= _shipController.ThrustBackwards;
        _inputReader.LeftThrustEvent -= _shipController.ThrustLeft;
        _inputReader.RightThrustEvent -= _shipController.ThrustRight;
        _inputReader.RotationThrustersEvent -= _shipController.RotationThrust;
        _inputReader.StepLeftEvent -= _shipController.StepLeft;
        _inputReader.StepRightEvent -= _shipController.StepRight;
        _inputReader.BoostEvent -= _shipController.Boost;
        _inputReader.BrakeEvent -= _shipController.Brake;
    }

    private void Start()
    {
        _cameraController.Init(GetComponent<ShipController>(), _cameraConstraint, _cameraLookAt);
        _speedometer.Init(GetComponent<Rigidbody>());
    }
}
