﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    GameObject _mainCamera;

    // Start is called before the first frame update
    void Start() {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Enter() {
        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, _mainCamera.transform.position.z);
    }
}
