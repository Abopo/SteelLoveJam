using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class TrueEnding : MonoBehaviour {

    DialogueRunner _dialogueRunner;

    // Start is called before the first frame update
    void Start() {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();

        _dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnNodeComplete(string node) {
        // Load credits scene
        SceneManager.LoadScene("Credits");
    }
}
