using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceForward : MonoBehaviour {

    Rigidbody _rBody;

    // Start is called before the first frame update
    void Start() {
        _rBody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (_rBody.velocity.magnitude > 0) {
            // Rotate to face towards the rigidbody's velocity vector
            transform.LookAt(transform.position + _rBody.velocity);
        }
    }
}
