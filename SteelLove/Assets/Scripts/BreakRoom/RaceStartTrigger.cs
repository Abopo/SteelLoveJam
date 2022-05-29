using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class RaceStartTrigger : Interactable {

    MainUI _mainUI;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        _mainUI = FindObjectOfType<MainUI>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact() {
        base.Interact();

        _mainUI.DisplayDialogue("NextRace");
    }
}
