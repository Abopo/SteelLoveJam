using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {

    AICheckpoint[] _aiCheckpoints;

    // Start is called before the first frame update
    void Start() {
        _aiCheckpoints = GetComponentsInChildren<AICheckpoint>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public AICheckpoint GetRandomCheckpoint() {
        int rand = Random.Range(0, _aiCheckpoints.Length-1);
        return _aiCheckpoints[rand];
    }
}
