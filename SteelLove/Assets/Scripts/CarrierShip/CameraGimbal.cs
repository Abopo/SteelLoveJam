using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGimbal : MonoBehaviour {

    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] float _mouseSensitivity = 1;
    float _xRot;
    float _yRot;
    [SerializeField] bool invert_x;
    [SerializeField] bool invert_y;

    Transform _innerGimbal;

    // Start is called before the first frame update
    void Start() {
        _innerGimbal = transform.Find("InnerGimbal");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnLook(Vector2 lookDelta) {
        if (lookDelta.x != 0) {
            int dir = 1;
            if (invert_x) {
                dir = -1;
            }

            _xRot += dir * lookDelta.x * _mouseSensitivity;
            transform.localRotation = Quaternion.Euler(0, _xRot, 0);

            //transform.Rotate(Vector3.up, dir * lookDelta.x * _mouseSensitivity, Space.Self);
			//rotate_object_local(Vector3.UP, dir* event.relative.x * mouse_sensitivity)
        }
        if (lookDelta.y != 0) {
            int dir = -1;
            if (invert_y) {
                dir = 1;
            }

            _yRot += dir * lookDelta.y * _mouseSensitivity;
            _yRot = Mathf.Clamp(_yRot, -60, 60);
            _innerGimbal.localRotation = Quaternion.Euler(_yRot, 0, 0);

            //float y_rot = Mathf.Clamp(lookDelta.y, -30, 30);
            //_innerGimbal.Rotate(Vector3.right, dir * y_rot * _mouseSensitivity, Space.Self);
            //float xClamp = ClampAngle(_innerGimbal.localEulerAngles.x, -75, 75);
            //_innerGimbal.localEulerAngles = new Vector3(xClamp, _innerGimbal.localEulerAngles.y, _innerGimbal.localEulerAngles.z);
			//$InnerGimbal.rotate_object_local(Vector3.RIGHT, dir * y_rotation * mouse_sensitivity);
        }
    }

    private void OnEnable() {
        _inputReader.LookEvent += OnLook;
    }

    private void OnDisable() {
        _inputReader.LookEvent -= OnLook;
    }
}
