using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class EndingScene : MonoBehaviour {

    DialogueRunner _dialogueRunner;
    bool _isComplete;
    
    [SerializeField] Material[] _skyboxes;

    float _waitTime = 5.0f;
    float _waitTimer = 0f;

    private void Awake() {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();
    }

    // Start is called before the first frame update
    void Start() {
        // TODO: Set ending text based on Ziv's ranking
        StartCoroutine(LateStart());

        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    IEnumerator LateStart() {
        yield return null;

        _dialogueRunner.StartDialogue("Ziv_Ending_Bad");
    }

    private void Update() {
        if (_isComplete) {
            _waitTimer += Time.deltaTime;
            if (_waitTimer > _waitTime) {
                // Load the main menu
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnDialogueComplete() {
        _isComplete = true;
    }
}
