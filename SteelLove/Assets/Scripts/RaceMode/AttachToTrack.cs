using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttachToTrack : MonoBehaviour
{
    [SerializeField] private float _raycastDist;
    [SerializeField] private float _floatHight;
    [SerializeField] private float _hightSmoothSpeed;
    [SerializeField] private float _normalSmoothSpeed;
    [SerializeField] private GameObject _centerPoint;

    private Vector3 _latestNormal;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Attach();
    }

    private void Attach()
    {
        RaycastHit hit;
        int mask = LayerMask.GetMask("TrackFloor");
        Ray rayForCast = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(rayForCast, out hit, _raycastDist, mask))
        {
            _latestNormal = hit.normal;
        }

        Quaternion fromTo = Quaternion.FromToRotation(_rigidBody.transform.up, _latestNormal);

        var newRot = Quaternion.Lerp(_rigidBody.rotation, fromTo * _rigidBody.rotation, Time.deltaTime * _normalSmoothSpeed);
        var newVel = Vector3.Lerp(_rigidBody.velocity, fromTo * _rigidBody.velocity, Time.deltaTime * _normalSmoothSpeed);

        _rigidBody.MoveRotation(newRot);
        _rigidBody.velocity = newVel;

        if (hit.collider != null)
        {
            _rigidBody.MovePosition(Vector3.Lerp(_rigidBody.position, hit.point + transform.up * _floatHight, Time.deltaTime * _hightSmoothSpeed));
        }

        if (_latestNormal == Vector3.up)
        {
            var flatVel = _rigidBody.velocity;
            var mag = flatVel.magnitude;
            flatVel.y = 0f;
            _rigidBody.velocity = flatVel.normalized * mag;
        }
    }
}
