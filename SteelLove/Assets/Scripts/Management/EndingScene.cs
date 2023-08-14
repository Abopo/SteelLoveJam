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

    private void Awake() {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();
        _lineView = GetComponentInChildren<MyLineView>();
    }

    void Start() {
        _inputReader.EnableAllInput();

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return null;

        // Set ending text based on Ziv's ranking
        int zivPos = GameManager.instance.GetZivPosition();

        if(zivPos == 0) {
            // True ending
            _dialogueRunner.StartDialogue("Ziv_Ending_True");
            RenderSettings.skybox = _skyboxes[2];
        } else if(zivPos <= 2) {
            // Top 3 ending
            _dialogueRunner.StartDialogue("Ziv_Ending_Good");
            RenderSettings.skybox = _skyboxes[2];
        } else if(zivPos <= 5) {
            // Mid ending
            _dialogueRunner.StartDialogue("Ziv_Ending_Mid");
            RenderSettings.skybox = _skyboxes[1];
        } else {
            // Bad ending
            _dialogueRunner.StartDialogue("Ziv_Ending_Bad");
            RenderSettings.skybox = _skyboxes[0];
        }
    }

    private void OnEnable() {
        _inputReader.InteractEvent += Interact;
    }

    private void OnDisable()
    {
        _inputReader.InteractEvent -= Interact;
    }

    private void Update() {
    }

    void Interact(float value) {
        if(_lineView.typewriterIsDone) {
            if (PlayerPrefs.GetInt("Has_DarkOrb", 0) > 0) {
                _sceneManager.LoadScene(_dsdScene);
            } else {
                _sceneManager.LoadScene(_mainMenu);
            }
        }
    }
}
