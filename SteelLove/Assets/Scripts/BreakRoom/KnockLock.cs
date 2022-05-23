using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockLock : Interactable {

    [SerializeField] int _id;

    [SerializeField] int _knocksNeeded;
    int _curKnocks;

    [SerializeField] float _knockTime = 0.5f; // Time within the next knock needs to happen before failure
    float _timer = 0f;

    [SerializeField] string _successNodeName;

    bool _unlocked = false;

    MainUI _mainUI;

    private void Awake() {
        _mainUI = FindObjectOfType<MainUI>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        // If this knocklock has been opened already
        if (PlayerPrefs.GetInt("KnockLock_" + _id, 0) > 0) {
            _unlocked = true;
        } else {
            _unlocked = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if(_curKnocks > 0) {
            _timer += Time.deltaTime;
            if(_timer >= _knockTime) {
                // Too much time between knocks, reset count
                _curKnocks = 0;
                _timer = 0f;
            }
        }
    }

    public override void Interact() {
        base.Interact();

        if(_unlocked) {
            return;
        }

        _curKnocks++;
        _timer = 0f;
        if(_curKnocks >= _knocksNeeded) {
            Unlock();
        }
    }

    void Unlock() {
        _unlocked = true;
        _curKnocks = 0;

        // Display success dialogue
        _mainUI.DisplayDialogue(_successNodeName);

        // Update prefs
        PlayerPrefs.SetInt("KnockLock_" + _id, 1);
    }
}
