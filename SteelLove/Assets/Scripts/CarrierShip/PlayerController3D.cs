using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour {

    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] private float walkSpeed;
    private Vector2 moveValue;
    private Vector3 moveDir;

    CharacterController _controller;

    private float _interactValue;
    public float InteractValue => _interactValue;

    public bool InControl => _inControl;
    bool _inControl = true;


    // Start is called before the first frame update
    void Start() {
        _inputReader.EnableAllInput();
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (_inControl) {
            PlayerMovement();
        }

    }
    void PlayerMovement() {
        moveDir.Normalize();

        moveDir = transform.right * moveValue.x + transform.forward * moveValue.y;
        _controller.Move(moveDir * walkSpeed * Time.deltaTime);

        // Gravity
        if (!_controller.isGrounded) {
            _controller.Move(new Vector3(0f, -9f * Time.deltaTime, 0f));
        }
    }

#region Input Events

    private void Movement(Vector2 value) {
        moveValue = value;
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
