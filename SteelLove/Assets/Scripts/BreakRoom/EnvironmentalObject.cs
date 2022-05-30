using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class EnvironmentalObject : Interactable {

    [SerializeField] string _name;

    MainUI _mainUI;
    AudioSource _audioSource;

    void Awake() {
        _mainUI = FindObjectOfType<MainUI>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact() {
        base.Interact();

        if (_name != "") {
            _mainUI.DisplayDialogue(_name);
        }
        if (_audioSource != null) {
            _audioSource.Play();
        }
    }
}
