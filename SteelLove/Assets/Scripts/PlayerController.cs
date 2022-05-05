using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Vector2 moveDir;
    [SerializeField] private float moveSpeed;

    [SerializeField] private InputReader _inputReader = default;


    // Start is called before the first frame update
    void Start() {
        _inputReader.EnableAllInput();
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
        PlayerMovement();
    }

    void PlayerMovement() {
        moveDir.Normalize();
        transform.Translate(moveDir.x * moveSpeed * Time.deltaTime,
                            moveDir.y * moveSpeed * Time.deltaTime,
                            0);
    }

#region Input Events

    private void Movement(Vector2 value) {
        moveDir = value;
        if(Mathf.Abs(moveDir.x) > 0.5f) {
            moveDir.x = Mathf.Sign(moveDir.x) * 1f;
        }
        if (Mathf.Abs(moveDir.y) > 0.5f) {
            moveDir.y = Mathf.Sign(moveDir.y) * 1f;
        }
    }

    private void Interact(float value) {

    }

#endregion
}
