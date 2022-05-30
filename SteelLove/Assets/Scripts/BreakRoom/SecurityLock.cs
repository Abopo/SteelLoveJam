using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityLock : Interactable {

    [SerializeField] string _successNodeName;

    PlayerController _player;
    MainUI _mainUI;

    private void Awake() {
        _player = FindObjectOfType<PlayerController>();
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
        if(!_player.facingRight) {
            _mainUI.DisplayDialogue(_successNodeName);
        } else {
            _mainUI.DisplayDialogue("Nothing");
        }
    }
}
