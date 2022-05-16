using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    // Start is called before the first frame update
    protected virtual void Start() {
        // Just make sure that we are interactable
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    // Update is called once per frame
    void Update() {
        
    }

    virtual public void Interact() {

    }
}
