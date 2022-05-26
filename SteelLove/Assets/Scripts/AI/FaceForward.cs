using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceForward : MonoBehaviour {

    Rigidbody _rBody;
    Vector3 _point;
    Vector3 _toPoint;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start() {
        _rBody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (_rBody.velocity.magnitude > 0) {
            // Rotate to face towards the rigidbody's velocity vector
            //transform.LookAt(transform.position + _rBody.velocity);

            //_point = transform.position + _rBody.velocity;
            //_toPoint = _point - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(_rBody.velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }
    }
}
