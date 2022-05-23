using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LOCKS { LEFTLOCK, RIGHTLOCK, TOPLOCK, BOTTOMLOCK }
public class Lockbox : MonoBehaviour {

    [SerializeField] int _id;
    [SerializeField] LOCKS[] _lockpattern;
    [SerializeField] string _successNodeName;

    LOCKS[] _openAttempt;
    int _openIndex = 0;

    bool _opened;

    MainUI _mainUI;

    private void Awake() {
        _mainUI = FindObjectOfType<MainUI>();
    }
    // Start is called before the first frame update
    void Start() {
        _openAttempt = new LOCKS[_lockpattern.Length];

        // If this lockbox has been opened already
        if(PlayerPrefs.GetInt("Lockbox_" + _id, 0) > 0) {
            _opened = true;
        } else {
            _opened = false;
        }
    }

    public void LockPressed(LOCKS lockSide) {
        if (_opened) {
            _mainUI.DisplayDialogue("Lockbox_Opened");
        } else {
            _openAttempt[_openIndex] = lockSide;
            _openIndex++;

            // If we've reached the size of our lock pattern
            if (_openIndex >= _lockpattern.Length) {
                // Check if our attempt was correct
                CheckAttempt();
                // Reset the box
                _openIndex = 0;
            }
        }
    }

    void CheckAttempt() {
        for(int i = 0; i < _lockpattern.Length; ++i) {
            if(_lockpattern[i] != _openAttempt[i]) {
                // Attempt failed
                AttemptFailed();
                return;
            }
        }

        // If we get here all the locks were correct
        AttemptSuccess();
    }

    void AttemptFailed() {
        // Play a failed sound

        // Display some dialogue?
        _mainUI.DisplayDialogue("Lockbox_Fail");
    }

    void AttemptSuccess() {
        // Play a success sound

        // Display success dialogue
        _mainUI.DisplayDialogue(_successNodeName);

        // Update prefs
        PlayerPrefs.SetInt("Lockbox_" + _id, 1);

        _opened = true;
    }
}
