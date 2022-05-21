using UnityEngine;

public class VelocityGraph : MonoBehaviour
{
    [SerializeField] private Transform _graph;
    [SerializeField] private Material _lineMat;
    [SerializeField] private float _lineThickness;
    [SerializeField] private float _lineScale;

    [SerializeField] private Rigidbody _playerRigidBody;

    private LineRenderer _lineRenderer;
    private GameObject _currentLineObject;

    private void Start()
    {
        _currentLineObject = new GameObject();
        _lineRenderer = _currentLineObject.AddComponent<LineRenderer>();
        _currentLineObject.transform.position = _graph.position;
        _lineRenderer.material = _lineMat;
        _lineRenderer.startWidth = _lineThickness;
        _lineRenderer.endWidth = _lineThickness;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(_playerRigidBody.velocity.x, _playerRigidBody.velocity.y, _playerRigidBody.velocity.z);
        Vector3 startPoint = _graph.position;
        Vector3 endPoint = _graph.position + velocity * _lineScale;
        _lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
    }
}
