using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour {

    [SerializeField] private float _mainthrustForce;
    [SerializeField] private float _reverseThrustForce;
    [SerializeField] private float _horizontalThrustForce;
    [SerializeField] private float _stepForce;

    [SerializeField] private float _rotForce;

    [SerializeField] private float _stepTime;
    [SerializeField] private float _speedLimitEnableAfterStepTime;

    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxRotSpeed;

    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] private VoidEventChannelSO _OnCrossedFinishLine = default;

    private Rigidbody2D _rigidbody2D = default;

    private float _mainThrusterInputValue;
    private float _reverseThrusterInputValue;
    private float _leftThrusterInputValue;
    private float _rightThrusterInputValue;
    private Vector2 _rotInputValue;

    private Vector2 _stepDir;
    private bool _speedLimitEnabled = true;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        PerformMovement();

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
    }

    private void PerformMovement()
    {
        Vector2 addingForce = transform.up * _mainthrustForce * _mainThrusterInputValue;
        _rigidbody2D.AddForce(addingForce);

        addingForce = -transform.up * _reverseThrustForce * _reverseThrusterInputValue;
        _rigidbody2D.AddForce(addingForce);

        addingForce = transform.right * _horizontalThrustForce * _leftThrusterInputValue;
        _rigidbody2D.AddForce(addingForce);

        addingForce = -transform.right * _horizontalThrustForce * _rightThrusterInputValue;
        _rigidbody2D.AddForce(addingForce);

        _rigidbody2D.AddTorque(_rotForce * -_rotInputValue.x);
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
#endregion
}
