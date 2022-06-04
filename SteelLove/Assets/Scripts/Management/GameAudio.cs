using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudio : MonoBehaviour {

    [SerializeField] SceneManagerSO _sceneManager;
    AudioSource _music;

    private AudioClip _previousClip;
    private bool _isPlayingSongThenReturn;

    // Start is called before the first frame update
    void Start() {
        _music = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    private void Update()
    {
        if (_isPlayingSongThenReturn)
        {
            if(_music.isPlaying == false)
            {
                _music.clip = _previousClip;
                _music.loop = true;
                _music.Play();
                _isPlayingSongThenReturn = false;
            }
        }
    }

    public void SetVolume(float volume) {
        _music.volume = volume;
    }

    public void PlaySongOnceThenReturn(AudioClip clip)
    {
        _previousClip = _music.clip;
        _music.Stop();
        _music.clip = clip;
        _music.Play();
        _music.loop = false;
        _isPlayingSongThenReturn = true;
    }

    void OnSceneLoaded(Scene oldScene, Scene newScene) {
        // Set music
        Debug.Log("Change music to: " + _sceneManager.currentScene.sceneMusic.ToString());
        _music.clip = _sceneManager.currentScene.sceneMusic;
        _music.volume = _sceneManager.currentScene.volume;
        _music.loop = true;
        _music.Play();
        _isPlayingSongThenReturn = false;
    }

    public void StopMusic() {
        _music.Stop();
    }
}
