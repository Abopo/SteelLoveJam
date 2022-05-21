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
            Debug.Log("hit");

            Quaternion fromTo = Quaternion.FromToRotation(_rigidBody.transform.up, hit.normal);
            
            _rigidBody.rotation = Quaternion.Lerp(_rigidBody.rotation, fromTo * _rigidBody.rotation, Time.deltaTime * _normalSmoothSpeed);
            _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, fromTo * _rigidBody.velocity, Time.deltaTime * _normalSmoothSpeed);
            _rigidBody.position = Vector3.Lerp(_rigidBody.position, hit.point + transform.up * _floatHight, Time.deltaTime * _hightSmoothSpeed);

            //transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * _normalSmoothTime);
            //transform.position = Vector3.Lerp(transform.position, hit.point + transform.up * _floatHight, Time.deltaTime * _hightSmoothTime);
            //var hitNormalRot = Quaternion.Euler(hit.normal);
            //var rotDiff = Quaternion.FromToRotation(transform.up, hit.normal);
            //transform.rotation = transform.rotation * rotDiff;
            //transform.position = hit.point + transform.up * _floatHight;
        }
    }
}
