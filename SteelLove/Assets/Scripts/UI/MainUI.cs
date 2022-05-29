using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour {

    [SerializeField] DialogueRunner _dialogueRunner;

    PlayerController _playerController;

    string _dialogueToRun = "";

    void Awake() {
        _playerController = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    void Start() {
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        _dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
    }

    // Update is called once per frame
    void Update() {
        // Make sure the player doesn't have control when dialogue is up
        if (_dialogueRunner.IsDialogueRunning) {
            if (_playerController.InControl) {
                _playerController.Freeze();
            }
        // and vice versa
        }// else {
        //    if (!_playerController.InControl) {
        //        _playerController.Unfreeze();
        //    }
        //}
    }

    // Displays dialogue via YarnSpinner, needs the name of the node to display
    public void DisplayDialogue(string nodeName) {
        if (!_dialogueRunner.IsDialogueRunning) {
            _dialogueRunner.StartDialogue(nodeName);
            _playerController.Freeze();
        } else {
            // Hold on to the node until the current dialogue is finished
            _dialogueToRun = nodeName;
        }
    }

    void OnDialogueComplete() {
        if(_dialogueToRun != "") {
            StartCoroutine(DisplayDialogueLater(_dialogueToRun));
            //DisplayDialogue(_dialogueToRun);
            _dialogueToRun = "";
        } else {
            _playerController.Unfreeze();
        }
    }

    // Even though the dialogue runner says it's NOT RUNNING, i still get an error when trying to run a new dialogue on the same frame as the last completed.
    // So wait a frame to make sure it's allllllllll done
    IEnumerator DisplayDialogueLater(string node) {
        yield return null;

        DisplayDialogue(node);
    }

    void OnNodeComplete(string node) {
        _playerController.Unfreeze();
    }

    [YarnCommand("get_item")]
    public static void GetItem(string itemName) {
        FindObjectOfType<MainUI>().DisplayDialogue(itemName);
    }

    [YarnCommand("start_race")]
    public static void StartNextRace() {
        // Load into the next race
        switch (GameManager.instance.NextRace) {
            case 1:
                SceneManager.LoadScene("Track2");
                break;
            case 2:
                SceneManager.LoadScene("Track3");
                break;
            case 3:
                SceneManager.LoadScene("Track4");
                break;
            case 4:
                SceneManager.LoadScene("Track5");
                break;
        }
    }

}
