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

    [SerializeField] SceneManagerSO _sceneManager;
    [SerializeField] GameSceneSO _mainMenu;
    [SerializeField] GameSceneSO _dsdScene;

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
            if (PlayerPrefs.GetInt("Has_DarkOrb", 0) > 0) {
                _sceneManager.LoadScene(_dsdScene);
            } else {
                // Load the main menu
                _sceneManager.LoadScene(_mainMenu);
                //SceneManager.LoadScene(0);
            }
        }
    }
}
