using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICheckpoint : MonoBehaviour {

    public AICheckpoint nextCheckpoint;
    public int id;

    public Vector3 center;

    private void Awake() {
        center = GetComponent<MeshCollider>().bounds.center;
    }
    // Start is called before the first frame update
    void Start() {
        //GameObject centerDebug = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //centerDebug.transform.position = new Vector3(center.x, 0.5f, center.z);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
