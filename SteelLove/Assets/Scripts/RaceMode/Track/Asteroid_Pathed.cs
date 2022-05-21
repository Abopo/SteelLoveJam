using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Pathed : MonoBehaviour
{

    [SerializeField] Transform _asteroid;

    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;

    [SerializeField] float _moveSpeed;

    // Start is called before the first frame update
    void Start() {
        _asteroid.position = _startPoint.position;
    }

    // Update is called once per frame
    void Update() {
        Debug.DrawLine(_startPoint.position, _endPoint.position);

        // Move back and forth between the points
        float time = Mathf.PingPong(Time.time * _moveSpeed, 1);
        _asteroid.position = Vector3.Lerp(_startPoint.position, _endPoint.position, time);
    }
}
