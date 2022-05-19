using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    int nextRace = 1; // What race is next to play
    public int NextRace { get => nextRace; }

    public static GameManager instance;

    bool _alive;


    void Awake() {
        SingletonCheck();

        if (_alive) {
            // Do initial setup stuff

        }
    }

    void SingletonCheck() {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        if (obj != null && obj != this.gameObject) {
            _alive = false;
            DestroyImmediate(this.gameObject);
        } else {
            instance = this;
            _alive = true;
        }
    }

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void RaceFinished() {
        // Update next race
        nextRace++;

        // Load Break Room
        SceneManager.LoadScene("BreakRoom");
    }
}
