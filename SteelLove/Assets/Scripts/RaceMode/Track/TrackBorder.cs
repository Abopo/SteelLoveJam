using UnityEngine;

public class TrackBorder : MonoBehaviour
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _spriteDistance;

    [SerializeField] private GameObject _borderObject;

    private GameObject _lineObj;

    private void Start()
    {
        if (_endPoint != null)
        {
            _lineObj = new GameObject();
            _lineObj.transform.parent = transform;
            _lineObj.transform.localPosition = Vector3.zero;

            float curDist = 0.0f;
            float distToCover = (_endPoint.position - transform.position).magnitude;
            Vector3 dirToEnd = (_endPoint.position - transform.position).normalized;
            while (curDist < distToCover)
            {
                var spawnPos = dirToEnd * curDist;
                var curBorderObj = Instantiate(_borderObject, _lineObj.transform);
                curBorderObj.transform.localPosition = spawnPos;

                curDist += _spriteDistance;
            }
        }
    }

    private void OnDestroy()
    {
        if (_lineObj != null)
        {
            foreach (Transform child in _lineObj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Destroy(_lineObj);
        }
    }
}
