using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable {

    [SerializeField] GameObject _dialogueBubble;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact() {
        base.Interact();

        // Open dialogue
        _dialogueBubble.SetActive(true);
    }
}
