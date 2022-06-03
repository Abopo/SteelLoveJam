using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour {

    [SerializeField] SceneManagerSO _sceneManager;
    AudioSource _music;

    // Start is called before the first frame update
    void Start() {
        _music = GetComponent<AudioSource>();
    }

    public void SetVolume(float volume) {
        _music.volume = volume;
    }

    void OnSceneLoaded() {
        // Set music
    }
}
