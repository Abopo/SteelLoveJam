using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGimbal : MonoBehaviour {

    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] float _mouseSensitivity = 1;
    float _xRot;
    float _xLerp;
    float _yRot;
    float _yLerp;
    [SerializeField] float acceleration;
    [SerializeField] bool invert_x;
    [SerializeField] bool invert_y;

    Transform _innerGimbal;

    // Start is called before the first frame update
    void Start() {
        _innerGimbal = transform.Find("InnerGimbal");
    }

    // Update is called once per frame
    void Update() {
        //_xLerp = Mathf.MoveTowards(_xLerp, _xRot, acceleration * Time.deltaTime);
        //transform.localRotation = Quaternion.Euler(0, _xLerp, 0);

        //_yLerp = Mathf.MoveTowards(_yLerp, _yRot, acceleration * Time.deltaTime);
        //_innerGimbal.localRotation = Quaternion.Euler(_yLerp, 0, 0);
    }

    void OnLook(Vector2 lookDelta) {
        Debug.Log("Mouse delta: " + lookDelta.ToString());

        if (lookDelta.x != 0) {
            int dir = 1;
            if (invert_x) {
                dir = -1;
            }

            _xRot += dir * lookDelta.x * _mouseSensitivity;
            transform.localRotation = Quaternion.Euler(0, _xRot, 0);
        }
        if (lookDelta.y != 0) {
            int dir = -1;
            if (invert_y) {
                dir = 1;
            }

            _yRot += dir * lookDelta.y * _mouseSensitivity;
            _yRot = Mathf.Clamp(_yRot, -60, 60);
            _innerGimbal.localRotation = Quaternion.Euler(_yRot, 0, 0);
        }
    }

    private void OnEnable() {
        _inputReader.LookEvent += OnLook;
    }

    private void OnDisable() {
        _inputReader.LookEvent -= OnLook;
    }
}
