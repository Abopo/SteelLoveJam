using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Engine_Hazard : MonoBehaviour {

    [SerializeField] ParticleSystem[] _particles;
    [SerializeField] float _activeTime;
    [SerializeField] Collider _hazardCollider;

    float _timer;

    bool _isActive = false;

    void Awake() {
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        _timer += Time.deltaTime;
        if (_timer > _activeTime) {
            if (!_isActive) {
                Activate();
            } else {
                Deactivate();
            }

            _timer = 0;
        }

    }

    void Activate() {
        foreach (ParticleSystem particle in _particles) {
            particle.Play();
        }

        // It takes a second for the particles to reach the full size of the collider,
        // so wait a little before enabling it to avoid unfair hits
        StartCoroutine(EnableColliderLater());

        _isActive = true;
    }

    IEnumerator EnableColliderLater() {
        yield return new WaitForSeconds(1);
        _hazardCollider.enabled = true;
    }

    void Deactivate() {
        foreach (ParticleSystem particle in _particles) {
            particle.Stop();
        }
        _hazardCollider.enabled = false;
        _isActive = false;
    }
}
