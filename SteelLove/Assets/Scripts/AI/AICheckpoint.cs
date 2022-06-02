using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICheckpoint : MonoBehaviour {

    public AICheckpoint nextCheckpoint;
    public int id;

    public float angleToNextCheckpoint;
    public Vector3 center;
    public int lookForwardAmount = 6;
    private void Awake() {
        center = GetComponent<MeshCollider>().bounds.center;
    }
    // Start is called before the first frame update
    void Start() {
        //GameObject centerDebug = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //centerDebug.GetComponent<Collider>().enabled = false;
        //centerDebug.transform.position = center;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
