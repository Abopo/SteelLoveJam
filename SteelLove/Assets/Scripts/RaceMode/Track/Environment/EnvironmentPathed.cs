using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPathed : MonoBehaviour
{

    [SerializeField] Transform _asteroid;

    [SerializeField] List<Transform> _path;

    [SerializeField] bool _loop;
    [SerializeField] bool _faceNextPoint;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;

    private int _lastPathInd = 0;
    private int _nextPathInd = 1;

    // Start is called before the first frame update
    void Start() {
        _asteroid.position = _path[0].position;
        if (_faceNextPoint)
        {
            _asteroid.rotation = _path[0].rotation;
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < _path.Count; i++)
        {
            int nextPoint = i + 1;
            if (i == _path.Count - 1)
            {
                nextPoint = 0;
            }

            Debug.DrawLine(_path[i].position, _path[nextPoint].position);
        }

        var posDiff = _path[_nextPathInd].position - _asteroid.position;
        var distance = posDiff.magnitude;
        var dir = posDiff.normalized;
        var movement = dir * _moveSpeed * Time.deltaTime;
        if (movement.magnitude > distance)
        {
            _asteroid.position = _path[_nextPathInd].position;
        }
        else
        {
            _asteroid.position = _asteroid.position + movement;
        }

        if (_faceNextPoint)
        {
            _asteroid.rotation = Quaternion.Lerp(_asteroid.rotation, _path[_lastPathInd].rotation, Time.deltaTime * _rotationSpeed);
        }

        if (_asteroid.position == _path[_nextPathInd].position)
        {
            var last = _lastPathInd;
            _lastPathInd = _nextPathInd;
            if(last < _nextPathInd)
            {
                _nextPathInd = _nextPathInd + 1;
            }
            else
            {
                _nextPathInd = _nextPathInd - 1;
            }

            if (_nextPathInd == _path.Count)
            {
                if(_loop)
                {
                    _nextPathInd = 0;
                }
                else
                {
                    _nextPathInd = _lastPathInd - 1;
                }
            }
            else if (_nextPathInd < 0)
            {
                _nextPathInd = 1;
            }
        }
    }
}
