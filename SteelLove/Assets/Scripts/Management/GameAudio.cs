using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudio : MonoBehaviour {

    [SerializeField] SceneManagerSO _sceneManager;
    AudioSource _music;

    // Start is called before the first frame update
    void Start() {
        _music = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    public void SetVolume(float volume) {
        _music.volume = volume;
    }

    void OnSceneLoaded(Scene oldScene, Scene newScene) {
        // Set music
        Debug.Log("Change music to: " + _sceneManager.currentScene.sceneMusic.ToString());
        _music.clip = _sceneManager.currentScene.sceneMusic;
        _music.Play();
    }

    public void StopMusic() {
        _music.Stop();
    }
}
