using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using TMPro;

public class EndingScene : MonoBehaviour {

    [SerializeField] private InputReader _inputReader = default;

    DialogueRunner _dialogueRunner;
    MyLineView _lineView;
    bool _isComplete;

    [SerializeField] Material[] _skyboxes;

    float _waitTime = 5.0f;
    float _waitTimer = 0f;

    private void Awake() {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();
        _lineView = GetComponentInChildren<MyLineView>();
    }

    // Start is called before the first frame update
    void Start() {
        _inputReader.EnableAllInput();

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return null;

        // TODO: Set ending text based on Ziv's ranking
        _dialogueRunner.StartDialogue("Ziv_Ending_Bad");
    }

    private void OnEnable() {
        _inputReader.InteractEvent += Interact;
    }

    private void Update() {
    }

    void Interact(float value) {
        if(_lineView.typewriterIsDone) {
            // Load the main menu
            SceneManager.LoadScene(0);
        }
    }
}
