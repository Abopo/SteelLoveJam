using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public enum STATE { TOP3 = 0, MID3, BOT3, NO_STATE };

public class Character : Interactable {

    [SerializeField] string _charaName;

    DialogueRunner _dialogueBubble;
    [SerializeField] GameObject _dialogueArrow;

    [SerializeField] Vector2[] _posList = new Vector2[4]; // The 4 positions this character will be in between each race

    public STATE curState;

    bool _isSpeaking;

    PlayerController _playerController;

    void Awake() { 
        _dialogueBubble = GetComponentInChildren<DialogueRunner>();
        _playerController = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    void Start() {
        curState = STATE.NO_STATE;

        _dialogueBubble.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    // Update is called once per frame
    void Update() {
    }

    public override void Interact() {
        if (!_isSpeaking) {
            base.Interact();

            // Freeze player while speaking
            _playerController.Freeze();

            BeginDialogue();
        }
    }

    void BeginDialogue() {
        string dialogueToSay = _charaName;

        // Choose correct dialogue
        switch (GameManager.instance.NextRace) {
            case 1:
                dialogueToSay += "_Race1";
                break;
            case 2:
                dialogueToSay += "_Race2";
                break;
            case 3:
                dialogueToSay += "_Race3";
                break;
            case 4:
                dialogueToSay += "_Race4";
                break;
        }

        switch(curState) {
            case STATE.TOP3:
                dialogueToSay += "_Top3";
                break;
            case STATE.MID3:
                dialogueToSay += "_Mid3";
                break;
            case STATE.BOT3:
                dialogueToSay += "_Bot3";
                break;
        }

        if (!_dialogueBubble.IsDialogueRunning) {
            _dialogueBubble.StartDialogue(dialogueToSay);
        }
    }

    void EndDialogue() {
        //_dialogueBubble.SetActive(false);
        _isSpeaking = false;
    }

    void OnDialogueComplete() {
        // Unfreeze player
        _playerController.Unfreeze();
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (_isSpeaking) {
            // Player walked away, stop speaking
            EndDialogue();
        }
    }
}
