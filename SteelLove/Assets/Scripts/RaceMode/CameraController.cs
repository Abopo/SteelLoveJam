using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ShipController _playerShipController;
    [SerializeField] private GameObject _cameraConstraint;
    [SerializeField] private GameObject _cameraLookAt;
    [SerializeField] private float _speed = 0;
    public float _defaultFOV = 80;

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, _cameraConstraint.transform.position, Time.deltaTime * _speed);
        transform.LookAt(_cameraLookAt.transform.position);
    }
}
