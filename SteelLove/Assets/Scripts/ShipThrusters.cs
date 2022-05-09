using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusters : MonoBehaviour {

    [SerializeField] ParticleSystem[] _backThrusters;
    [SerializeField] ParticleSystem _leftThruster;
    [SerializeField] ParticleSystem _rightThruster;
    [SerializeField] ParticleSystem _leftNoseThruster;
    [SerializeField] ParticleSystem _rightNoseThruster;


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
            _leftThruster.Play();
        } else {
            _leftThruster.Stop();
        }
    }

    public void RightThrusters(float value) {
        if (value > 0) {
            _rightThruster.Play();
        } else {
            _rightThruster.Stop();
        }
    }

    public void NoseThrusters(float value) {
        if(value > 0) {
            _leftNoseThruster.Play();
            _rightNoseThruster.Stop();
        } else if(value < 0) {
            _rightNoseThruster.Play();
            _leftNoseThruster.Stop();
        } else {
            _rightNoseThruster.Stop();
            _leftNoseThruster.Stop();
        }
    }
}
