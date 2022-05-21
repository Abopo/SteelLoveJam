using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate3D : MonoBehaviour {

    [SerializeField] float rotSpeed;

    [Range(0f, 1f)]
    [SerializeField] float xRot;
    [Range(0f, 1f)]
    [SerializeField] float yRot;
    [Range(0f, 1f)]
    [SerializeField] float zRot;

    // Update is called once per frame
    void Update() {
        transform.Rotate(xRot * rotSpeed * Time.deltaTime, 
                         yRot * rotSpeed * Time.deltaTime,
                         zRot * rotSpeed * Time.deltaTime); 
    }
}
