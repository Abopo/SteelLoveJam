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

    [SerializeField] private List<Transform> _attachPoints;

    public Vector3 LatestNormal => _latestNormal;
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

        Vector3 averagedNormals = new Vector3();
        int normalCount = 0;
        bool hitFloor = false;

        foreach (var point in _attachPoints)
        {
            Ray pointCast = new Ray(point.position, -transform.up);
            if (Physics.Raycast(pointCast, out hit, _raycastDist, mask))
            {
                averagedNormals += hit.normal;
                normalCount++;
                hitFloor = true;
            }
        }
        averagedNormals = averagedNormals / normalCount;
        
        if (hitFloor)
        {
            SetHeight();
            FaceVelocity(averagedNormals);

            _latestNormal = averagedNormals;
        }
        else
        {
            FaceVelocity(_latestNormal);
        }

        ContinueRotatingToNormal();
    }

    private void SetHeight()
    {
        RaycastHit hit;
        int mask = LayerMask.GetMask("TrackFloor");
        Ray rayCast = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(rayCast, out hit, _raycastDist, mask))
        {
            _rigidBody.MovePosition(Vector3.Lerp(_rigidBody.position, hit.point + transform.up * _floatHight, Time.deltaTime * _hightSmoothSpeed));
        }
    }

    private void FaceVelocity(Vector3 normal)
    {
        var velocity = _rigidBody.velocity;
        var velMag = velocity.magnitude;
        Vector3.OrthoNormalize(ref normal, ref velocity);

        _rigidBody.velocity = velocity * velMag;
    }

    private void ContinueRotatingToNormal()
    {
        Quaternion fromTo = Quaternion.FromToRotation(_rigidBody.transform.up, _latestNormal);
        var newRot = Quaternion.Lerp(_rigidBody.rotation, fromTo * _rigidBody.rotation, Time.deltaTime * _normalSmoothSpeed);
        _rigidBody.MoveRotation(newRot);
    }
}
