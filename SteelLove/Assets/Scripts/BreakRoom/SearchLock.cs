using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLock : Interactable {

    [SerializeField] int _id;

    [SerializeField] int _searchesNeeded;
    int _curSearches;

    [SerializeField] string _successNodeName;

    bool _unlocked = false;

    MainUI _mainUI;

    private void Awake() {
        _mainUI = FindObjectOfType<MainUI>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        // If this SearchLock has been opened already
        if (PlayerPrefs.GetInt("SearchLock_" + _id, 0) > 0) {
            _unlocked = true;
        } else {
            _unlocked = false;
        }
    }

    public override void Interact() {
        base.Interact();

        if (_unlocked) {
            return;
        }

        _curSearches++;
        if (_curSearches >= _searchesNeeded) {
            Unlock();
        } else {
            _mainUI.DisplayDialogue("Nothing");
        }
    }

    void Unlock() {
        _unlocked = true;
        _curSearches = 0;

        // Display success dialogue
        _mainUI.DisplayDialogue(_successNodeName);

        // Update prefs
        PlayerPrefs.SetInt("SearchLock_" + _id, 1);
    }
}
