using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_Hazard : Hazard {

    [SerializeField] GameObject _mesh;
    [SerializeField] ParticleSystem _particles;

    // Start is called before the first frame update
    void Start() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Ship") {
            // Ship collision handles damage, so just destruct
            _mesh.SetActive(false);
            _particles.Play();
        }
    }

}
