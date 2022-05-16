using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MainUI : MonoBehaviour {

    [SerializeField] DialogueRunner _dialogueRunner;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Displays dialogue via YarnSpinner, needs the name of the node to display
    public void DisplayDialogue(string nodeName) {
        if (!_dialogueRunner.IsDialogueRunning) {
            _dialogueRunner.StartDialogue(nodeName);
        }
    }
}
