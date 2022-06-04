using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class TrueEnding : MonoBehaviour {

    DialogueRunner _dialogueRunner;

    public bool isDark;
    [SerializeField] private SceneManagerSO _sceneManager;
    [SerializeField] private TrackSceneSO _track;
    [SerializeField] private GameSceneSO _credits;

    // Start is called before the first frame update
    void Start() {
        _dialogueRunner = GetComponentInChildren<DialogueRunner>();

        _dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnNodeComplete(string node) {
        if (isDark) {
            PlayerPrefs.SetInt("DarkShippieDues", 1);
            _sceneManager.LoadScene(_track, false);
        } else {
            // Load credits scene
            _sceneManager.LoadScene(_credits, false);
        }
    }
}
