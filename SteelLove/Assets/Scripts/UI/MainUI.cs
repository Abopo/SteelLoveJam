using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MainUI : MonoBehaviour {

    [SerializeField] DialogueRunner _dialogueRunner;

    PlayerController _playerController;


    void Awake() {
        _playerController = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    void Start() {
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    // Update is called once per frame
    void Update() {
        // Make sure the player doesn't have control when dialogue is up
        if (_dialogueRunner.IsDialogueRunning) {
            if (_playerController.InControl) {
                _playerController.Freeze();
            }
        // and vice versa
        } else {
            if (!_playerController.InControl) {
                _playerController.Unfreeze();
            }
        }
    }

    // Displays dialogue via YarnSpinner, needs the name of the node to display
    public void DisplayDialogue(string nodeName) {
        if (!_dialogueRunner.IsDialogueRunning) {
            _dialogueRunner.StartDialogue(nodeName);
            _playerController.Freeze();
        }
    }

    void OnDialogueComplete() {
        _playerController.Unfreeze();
    }

    [YarnCommand("get_item")]
    public static void GetItem(string itemName) {
        FindObjectOfType<MainUI>().DisplayDialogue(itemName);
    }
}
