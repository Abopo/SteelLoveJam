using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalObject : Interactable {

    [SerializeField] string _name;

    MainUI _mainUI;

    void Awake() {
        _mainUI = FindObjectOfType<MainUI>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact() {
        base.Interact();

        _mainUI.DisplayDialogue(_name);
    }
}
