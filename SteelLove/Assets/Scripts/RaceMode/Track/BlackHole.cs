using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    [SerializeField] float _gravityForce;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Ship") {
            // Pull ship gradually towards center
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            Vector3 toCenter = transform.position - other.transform.position;
            rb.AddForce(_gravityForce * toCenter.normalized);
        }
    }
}
