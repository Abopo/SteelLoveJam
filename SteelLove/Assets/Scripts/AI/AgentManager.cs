using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {

    [SerializeField] Transform checkpointsParent;
    AICheckpoint[] _aiCheckpoints;

    private void Awake() {
        InitializeCheckpoints();
    }
    // Start is called before the first frame update
    void Start() {
    }

    void InitializeCheckpoints() {
        _aiCheckpoints = new AICheckpoint[checkpointsParent.childCount];

        for (int i = 0; i < _aiCheckpoints.Length; i++) {
            _aiCheckpoints[i] = checkpointsParent.GetChild(i).GetComponent<AICheckpoint>();

            _aiCheckpoints[i].id = i;

            if (i < _aiCheckpoints.Length - 1) {
                _aiCheckpoints[i].nextCheckpoint = checkpointsParent.GetChild(i+1).GetComponent<AICheckpoint>();
            } else {
                _aiCheckpoints[i].nextCheckpoint = _aiCheckpoints[0];
            }
        }
    }

    public AICheckpoint GetRandomCheckpoint() {
        int rand = Random.Range(0, _aiCheckpoints.Length-1);
        return _aiCheckpoints[rand];
    }

    public AICheckpoint FirstCheckpoint() {
        return _aiCheckpoints[0];
    }
}
