using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct dialogue {
}

public enum STATE { TOP3 = 0, MID3, BOT3, NO_STATE };

public class Character : Interactable {

    [SerializeField] GameObject _dialogueBubble;
    [SerializeField] GameObject _dialogueArrow;

    // Dialogue Lists
    List<string> race1 = new List<string>();
    List<string>[] race2 = new List<string>[3];
    List<string>[] race3 = new List<string>[3];
    List<string>[] race4 = new List<string>[3];

    public STATE curState;

    TMP_Text _dialogueText;

    bool _isSpeaking;
    List<string> _curDialogue;
    int _dialogueIndex = 0;

    // Start is called before the first frame update
    void Start() {
        _dialogueText = _dialogueBubble.GetComponentInChildren<TMP_Text>();
        SetDialogue();
    }

    public virtual void SetDialogue() {
        // Race 1
        race1.Add("Welcome to the game!");
        race1.Add("This is a test dialogue. It's very long to test the skip to end functionality.");
        race1.Add("Did it work?");

        // Race 2
        race2[(int)STATE.TOP3] = new List<string>();
        race2[(int)STATE.TOP3].Add("Nice first race!");
        race2[(int)STATE.TOP3].Add("I'm in the top 3!");

        race2[(int)STATE.MID3] = new List<string>();
        race2[(int)STATE.MID3].Add("The first race went OK.");
        race2[(int)STATE.MID3].Add("I'm in the middle of the pack.");

        race2[(int)STATE.BOT3] = new List<string>();
        race2[(int)STATE.BOT3].Add("The first race went terribly...");
        race2[(int)STATE.BOT3].Add("I need to really step it up if I'm gonna win.");
    }

    // Update is called once per frame
    void Update() {
    }

    public override void Interact() {
        if (!_isSpeaking) {
            base.Interact();

            BeginDialogue();
        } else if (IsTyping()) {
            // Skip text to the end
            _dialogueText.GetComponent<Typewriter>().SkipToEnd();
        } else if (_dialogueIndex < _curDialogue.Count) {
            // Disable the text obj
            _dialogueText.gameObject.SetActive(false);
            // Change the dialogue
            _dialogueText.text = _curDialogue[_dialogueIndex++];
            // Reenable the text obj
            _dialogueText.gameObject.SetActive(true);

            if(_dialogueIndex >= _curDialogue.Count) {
                // There's no more text after this, so turn off the arrow
                _dialogueArrow.SetActive(false);
            }

        } else {
            EndDialogue();
        }
    }

    void BeginDialogue() {
        // Choose correct dialogue
        switch (GameManager.instance.NextRace) {
            case 1:
                _curDialogue = race1;
                break;
            case 2:
                _curDialogue = race2[(int)curState];
                break;
            case 3:
                _curDialogue = race3[(int)curState];
                break;
            case 4:
                _curDialogue = race4[(int)curState];
                break;
        }

        _dialogueIndex = 0;
        _dialogueText.text = _curDialogue[_dialogueIndex++];

        // Open dialogue
        _dialogueBubble.SetActive(true);
        if (_dialogueIndex >= _curDialogue.Count) {
            // There's no more text after this, so turn off the arrow
            _dialogueArrow.SetActive(false);
        } else {
            _dialogueArrow.SetActive(true);
        }
        _isSpeaking = true;
    }

    bool IsTyping() {
        return !_dialogueText.GetComponent<Typewriter>().isDone;
    }

    void EndDialogue() {
        _dialogueBubble.SetActive(false);
        _isSpeaking = false;
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (_isSpeaking) {
            // Player walked away, stop speaking
            EndDialogue();
        }
    }
}
