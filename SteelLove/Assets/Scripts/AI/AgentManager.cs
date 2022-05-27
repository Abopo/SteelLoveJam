using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {

    AICheckpoint[] _aiCheckpoints;

    // Start is called before the first frame update
    void Start() {
        _aiCheckpoints = GetComponentsInChildren<AICheckpoint>();
        InitializeCheckpoints();
    }

    void InitializeCheckpoints() {
        for (int i = 0; i < _aiCheckpoints.Length; i++) {
            _aiCheckpoints[i].index = i;
            if (i < _aiCheckpoints.Length - 1) {
                _aiCheckpoints[i].nextCheckpoint = _aiCheckpoints[i + 1];
            }
        }
    }

    public AICheckpoint GetRandomCheckpoint() {
        int rand = Random.Range(0, _aiCheckpoints.Length-1);
        return _aiCheckpoints[rand];
    }
}
