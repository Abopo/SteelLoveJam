﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    private Vector2 moveDir;

    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] private Room myRoom;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    MainUI _mainUI;
    [SerializeField] STATE _curState; // Ziv's current ranking placement

    bool _inControl = true;

    void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponentInChildren<Collider2D>();

        _mainUI = FindObjectOfType<MainUI>();
    }
    // Start is called before the first frame update
    void Start() {
        _inputReader.EnableAllInput();

        // Ziv starts in his room, so make sure it is active
        myRoom.Enter();

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return null;

        // Say initial dialogue
        RunStartDialogue();
    }

    // Plays a dialogue based on the Race# and Ziv's current rank
    void RunStartDialogue() {
        string dialogueToRun = "Ziv_";

        switch(GameManager.instance.NextRace) {
            case 1:
                dialogueToRun += "Race1";
                break;
            case 2:
                dialogueToRun += "Race2";
                break;
            case 3:
                dialogueToRun += "Race3";
                break;
            case 4:
                dialogueToRun += "Race4";
                break;
        }

        switch (_curState) {
            case STATE.TOP3:
                dialogueToRun += "_Top3";
                break;
            case STATE.MID3:
                dialogueToRun += "_Mid3";
                break;
            case STATE.BOT3:
                dialogueToRun += "_Bot3";
                break;
        }

        _mainUI.DisplayDialogue(dialogueToRun);
    }

    private void OnEnable() {
        _inputReader.MovementEvent += Movement;
        _inputReader.InteractEvent += Interact;
    }

    private void OnDisable() {
        _inputReader.MovementEvent -= Movement;
        _inputReader.InteractEvent -= Interact;
    }

    // Update is called once per frame
    void Update() {
        if (_inControl) {
            PlayerMovement();
        }
    }

    void PlayerMovement() {
        moveDir.Normalize();
        _rigidbody2D.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

#region Input Events

    private void Movement(Vector2 value) {
        moveDir = value;
        if(Mathf.Abs(moveDir.x) > 0.5f) {
            moveDir.x = Mathf.Sign(moveDir.x) * 1f;
        } else {
            moveDir.x = 0f;
        }
        if (Mathf.Abs(moveDir.y) > 0.5f) {
            moveDir.y = Mathf.Sign(moveDir.y) * 1f;
        } else {
            moveDir.y = 0f;
        }
    }

    private void Interact(float value) {
        CheckInteract();
    }

#endregion

    private void CheckInteract() {
        // Check is we are overlapped with any interactables
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(LayerMask.GetMask("Interactable"));
        List<Collider2D> overlappingColliders = new List<Collider2D>();

        _collider2D.OverlapCollider(contactFilter, overlappingColliders);

        foreach (Collider2D collider in overlappingColliders) {
            collider.GetComponent<Interactable>().Interact();
        }
    }

    public void Freeze() {
        _inControl = false;
    }

    public void Unfreeze() {
        _inControl = true;
    }
}
