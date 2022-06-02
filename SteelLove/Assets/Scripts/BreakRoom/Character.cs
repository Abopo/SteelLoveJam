using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public enum STATE { NO_STATE, TOP3, MID3, BOT3 };

public class Character : Interactable {

    [SerializeField] string _charaName;

    DialogueRunner _dialogueBubble;

    [SerializeField] Vector2[] _posList = new Vector2[4]; // The 4 positions this character will be in between each race

    public STATE curState;

    bool _isSpeaking;

    PlayerController _playerController;

    void Awake() { 
        _dialogueBubble = GetComponentInChildren<DialogueRunner>();
        _playerController = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        CheckState();

        // Move to proper position depending on what race is next
        FindPosition();

        _dialogueBubble.onDialogueComplete.AddListener(OnDialogueComplete);
        _dialogueBubble.onNodeComplete.AddListener(OnNodeComplete);
    }

    private void CheckState() {
        // If this is the first race
        if(GameManager.instance.NextRace == 1 || GameManager.instance.NextRace == 5) {
            // There aren't any rankings yet, so stay in NO_STATE
            curState = STATE.NO_STATE;
        } else {
            // Check the ranking list for our character and set our state
            curState = GameManager.instance.GetCharacterState(_charaName);
        }
    }

    void FindPosition() {
        int nextRace = GameManager.instance.NextRace;
        if (nextRace < _posList.Length) {
            Vector3 position = _posList[nextRace - 1];
            position.z = -1;

            transform.localPosition = position;
        }
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

    void OnNodeComplete(string node) {
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
