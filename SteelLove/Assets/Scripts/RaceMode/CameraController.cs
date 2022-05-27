using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ShipController _playerShipController;
    [SerializeField] private GameObject _cameraConstraintLookingForward;
    [SerializeField] private GameObject _cameraConstraintLookingBackward;
    [SerializeField] private GameObject _cameraLookAtForward;
    [SerializeField] private GameObject _cameraLookAtBehind;
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _defaultFOV = 80;
    [SerializeField] private float _boostFOV = 110;
    [SerializeField] private float _smoothTime = 1;


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
}
