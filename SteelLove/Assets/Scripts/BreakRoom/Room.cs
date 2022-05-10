using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    GameObject _mainCamera;

    void Awake() {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Enter() {
        gameObject.SetActive(true);
        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, _mainCamera.transform.position.z);
    }

    public void Exit() {
        gameObject.SetActive(false);
    }
}
