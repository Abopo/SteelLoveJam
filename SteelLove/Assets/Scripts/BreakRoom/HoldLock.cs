using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldLock : Interactable {

    [SerializeField] float _holdTime;
    float _holdTimer = 0f;
    [SerializeField] string _successNodeName;

    bool _isHolding;
    bool _unlocked = false;

    PlayerController _player;
    MainUI _mainUI;

    private void Awake() {
        _player = FindObjectOfType<PlayerController>();
        _mainUI = FindObjectOfType<MainUI>();
    }
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        // If this knocklock has been opened already
        if (PlayerPrefs.GetInt("HoldLock", 0) > 0) {
            _unlocked = true;
        } else {
            _unlocked = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (_isHolding) {
            if (_player.InteractValue > 0) {
                _holdTimer += Time.deltaTime;
                if (_holdTimer > _holdTime) {
                    // Unlock
                    _mainUI.DisplayDialogue(_successNodeName);
                    _unlocked = true;
                    _isHolding = false;
                }
            } else {
                _isHolding = false;
                _holdTimer = 0f;
            }
        }
    }

    public override void Interact() {
        if (!_unlocked) {
            _isHolding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // Cancel hold
        _isHolding = false;
        _holdTimer = 0f;
    }
}
