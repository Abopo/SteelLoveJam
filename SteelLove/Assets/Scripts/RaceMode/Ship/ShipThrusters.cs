using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusters : MonoBehaviour {

    [SerializeField] ParticleSystem[] _backThrusters;
    [SerializeField] ParticleSystem[] _leftThrusters;
    [SerializeField] ParticleSystem[] _rightThrusters;
    [SerializeField] ParticleSystem[] _leftTurnThrusters;
    [SerializeField] ParticleSystem[] _rightTurnThrusters;
    [SerializeField] ParticleSystem[] _noseThrusters;
    [SerializeField] ParticleSystem _boostThruster;

    AudioSource _audioSource;
    ShipAudio _shipAudio;
    float _thrusterVolume = 0.5f;

    // Start is called before the first frame update
    void Start() {
        _shipAudio = FindObjectOfType<ShipAudio>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void BackThrusters(float value) {
        if(value > 0) {
            PlayThrusters(_backThrusters);
        } else if(_backThrusters[0].isPlaying) {
            StopThrusters(_backThrusters);
        }
    }

    public void LeftThrusters(float value) {
        if (value > 0) {
            PlayThrusters(_leftThrusters);
        } else if(_leftThrusters[0].isPlaying){
            StopThrusters(_leftThrusters);
        }
    }

    public void RightThrusters(float value) {
        if (value > 0) {
            PlayThrusters(_rightThrusters);
        } else if(_rightThrusters[0].isPlaying){
            StopThrusters(_rightThrusters);
        }
    }

    public void TurnThrusters(float value) {
        if(value > 0) {
            PlayThrusters(_leftTurnThrusters);
            StopThrusters(_rightTurnThrusters);
        } else if(value < 0) {
            PlayThrusters(_rightTurnThrusters);
            StopThrusters(_leftTurnThrusters);
        } else {
            StopThrusters(_rightTurnThrusters);
            StopThrusters(_leftTurnThrusters);
        }
    }

    public void NoseThrusters(float value) {
        if(value > 0) {
            PlayThrusters(_noseThrusters);
        } else if(_noseThrusters[0].isPlaying){
            StopThrusters(_noseThrusters);
        }
    }

    public void BoostThruster(bool boosting) {
        if(_boostThruster == null) {
            return;
        }

        if (boosting) {
            if (!_boostThruster.isPlaying) {
                _boostThruster.Play();
                _shipAudio.PlayBoostSound();
                BackThrusters(0);
            }
        } else if (_boostThruster.isPlaying) {
            _shipAudio.StopBoostSound();
            _boostThruster.Stop();
        }
    }

    void PlayThrusters(ParticleSystem[] thrusters) {
        foreach(ParticleSystem pS in thrusters) {
            pS.Play();
        }

        _audioSource = thrusters[0].GetComponent<AudioSource>();
        if (_audioSource != null && !_audioSource.isPlaying) {
            _audioSource.volume = _thrusterVolume;
            _audioSource.Play();
        }
    }

    void StopThrusters(ParticleSystem[] thrusters) {
        foreach (ParticleSystem pS in thrusters) {
            pS.Stop();
        }

        _audioSource = thrusters[0].GetComponent<AudioSource>();
        if (_audioSource != null) {
            StartCoroutine(AudioFadeOut.FadeOut(_audioSource, 0.05f));
        }
    }
}
