using UnityEngine;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public struct CameraPresets
    {
        public string name;
        public float speed;
        public float defaultFOV;
        public float boostFOV;
        public float smoothTime;
        public Vector3 cameraLookAtForwardPos;
        public Vector3 cameraLookAtBackwardPos;
        public Vector3 cameraPosDefaultForward;
        public Vector3 cameraPosDefaultBackward;
    }

    [SerializeField] private ShipController _playerShipController;
    [SerializeField] private GameObject _cameraConstraintLookingForward;
    [SerializeField] private GameObject _cameraConstraintLookingBackward;
    [SerializeField] private GameObject _cameraLookAtForward;
    [SerializeField] private GameObject _cameraLookAtBehind;
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _defaultFOV = 80;
    [SerializeField] private float _boostFOV = 110;
    [SerializeField] private float _smoothTime = 1;

    [SerializeField] private CameraPresets[] _cameraPresets;

    private int currentPreset = 0;

    private Camera ourCamera;

    private bool _lookingBehind;

    private void Awake()
    {
        ourCamera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (_playerShipController != null)
        {
            Follow();
            BoostFOV();
        }
    }

    public void Init(ShipController shipController, GameObject cameraConstraintLookingForward, GameObject cameraConstraintLookingBackwards, GameObject cameraLookAtForward, GameObject cameraLookAtBehind)
    {
        _playerShipController = shipController;
        _cameraConstraintLookingForward = cameraConstraintLookingForward;
        _cameraConstraintLookingBackward = cameraConstraintLookingBackwards;
        _cameraLookAtForward = cameraLookAtForward;
        _cameraLookAtBehind = cameraLookAtBehind;

        ApplyPreset();
    }

    public void LookBehind(float inputValue)
    {
        if (inputValue > 0)
        {
            _lookingBehind = true;
        }
        else
        {
            _lookingBehind = false;
        }
    }

    public void CyclePreset()
    {
        currentPreset++;
        if (currentPreset >= _cameraPresets.Length)
        {
            currentPreset = 0;
        }

        ApplyPreset();
    }

    private void Follow()
    {
        Transform cameraConstraint = _cameraConstraintLookingForward.transform;
        Vector3 lookAtPos = _cameraLookAtForward.transform.position;
        if(_lookingBehind)
        {
            cameraConstraint = _cameraConstraintLookingBackward.transform;
            lookAtPos = _cameraLookAtBehind.transform.position;
        }

        transform.position = Vector3.Lerp(transform.position, cameraConstraint.position, Time.deltaTime * _speed);
        transform.LookAt(lookAtPos, cameraConstraint.up);
    }

    private void BoostFOV()
    {
        if (_playerShipController.Boosting)
        {
            ourCamera.fieldOfView = Mathf.Lerp(ourCamera.fieldOfView, _boostFOV, Time.deltaTime * _smoothTime);
        }
        else
        {
            ourCamera.fieldOfView = Mathf.Lerp(ourCamera.fieldOfView, _defaultFOV, Time.deltaTime * _smoothTime);
        }
    }

    private void ApplyPreset()
    {
        var curPreset = _cameraPresets[currentPreset];
        _speed = curPreset.speed;
        _defaultFOV = curPreset.defaultFOV;
        _boostFOV = curPreset.boostFOV;
        _smoothTime = curPreset.smoothTime;
        _cameraLookAtForward.transform.localPosition = curPreset.cameraLookAtForwardPos;
        _cameraLookAtBehind.transform.localPosition = curPreset.cameraLookAtBackwardPos;
        _cameraConstraintLookingForward.transform.localPosition = curPreset.cameraPosDefaultForward;
        _cameraConstraintLookingBackward.transform.localPosition = curPreset.cameraPosDefaultBackward;

        if (_lookingBehind == false)
        {
            ourCamera.transform.localPosition = curPreset.cameraPosDefaultForward;
        } else
        {
            ourCamera.transform.localPosition = curPreset.cameraPosDefaultBackward;
        }
    }
}
