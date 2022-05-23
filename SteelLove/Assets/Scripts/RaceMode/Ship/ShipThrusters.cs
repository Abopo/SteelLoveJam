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


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void BackThrusters(float value) {
        if(value > 0) {
            _backThrusters[0].Play();
            _backThrusters[1].Play();
        } else {
            _backThrusters[0].Stop();
            _backThrusters[1].Stop();
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
