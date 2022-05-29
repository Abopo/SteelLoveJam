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

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void BackThrusters(float value) {
        if(value > 0) {
            PlayThrusters(_backThrusters);
        } else {
            StopThrusters(_backThrusters);
        }
    }

    public void LeftThrusters(float value) {
        if (value > 0) {
            PlayThrusters(_leftThrusters);
        } else {
            StopThrusters(_leftThrusters);
        }
    }

    public void RightThrusters(float value) {
        if (value > 0) {
            PlayThrusters(_rightThrusters);
        } else {
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
        } else {
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
                BackThrusters(0);
            }
        } else if (_boostThruster.isPlaying) {
            _boostThruster.Stop();
        }
    }

    void PlayThrusters(ParticleSystem[] thrusters) {
        foreach(ParticleSystem pS in thrusters) {
            pS.Play();
        }
    }

    void StopThrusters(ParticleSystem[] thrusters) {
        foreach (ParticleSystem pS in thrusters) {
            pS.Stop();
        }
    }
}
