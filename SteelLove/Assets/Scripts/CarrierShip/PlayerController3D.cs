using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour {

    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] private float walkForce;
    [SerializeField] private float walkSpeed;
    private Vector2 moveDir;

    Rigidbody _rigidBody;

    [SerializeField] private float _interactValue;
    public float InteractValue => _interactValue;

    public bool InControl => _inControl;
    bool _inControl = true;


    // Start is called before the first frame update
    void Start() {
        _inputReader.EnableAllInput();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (_inControl) {
            PlayerMovement();
        }

    }
    void PlayerMovement() {
        moveDir.Normalize();
        _rigidBody.velocity = new Vector3(moveDir.x * walkSpeed, 0, moveDir.y * walkSpeed);
    }

#region Input Events

    private void Movement(Vector2 value) {
        moveDir = value;
        if (Mathf.Abs(moveDir.x) > 0.5f) {
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
        _interactValue = value;

        if (value > 0) {
            if (_inControl) {
                CheckInteract();
            }
        }
    }

#endregion

    private void CheckInteract() {
        // Check if we are looking at any interactables
        
    }

    private void OnEnable() {
        _inputReader.MovementEvent += Movement;
        _inputReader.InteractEvent += Interact;
    }

    private void OnDisable() {
        _inputReader.MovementEvent -= Movement;
        _inputReader.InteractEvent -= Interact;
    }

}
