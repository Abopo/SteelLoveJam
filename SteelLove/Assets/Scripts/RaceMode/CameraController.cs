﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ShipController _playerShipController;
    [SerializeField] private GameObject _cameraConstraint;
    [SerializeField] private GameObject _cameraLookAt;
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _defaultFOV = 80;
    [SerializeField] private float _boostFOV = 110;
    [SerializeField] private float _smoothTime = 1;


    private Camera ourCamera;

    private void Awake()
    {
        ourCamera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Follow();
        BoostFOV();
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, _cameraConstraint.transform.position, Time.deltaTime * _speed);
        transform.LookAt(_cameraLookAt.transform.position);
    }

    private void BoostFOV()
    {
        if (_playerShipController.Boosting)
        {
            ourCamera.fieldOfView = Mathf.Lerp(ourCamera.fieldOfView, _boostFOV, Time.deltaTime * _smoothTime);
        }
        else
        {
            ourCamera.fieldOfView = Mathf.Lerp(ourCamera.fieldOfView, _defaultFOV, Time.deltaTime * _smoothTime);
        }
    }
}