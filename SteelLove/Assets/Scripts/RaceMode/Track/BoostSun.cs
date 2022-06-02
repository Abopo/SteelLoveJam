using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSun : MonoBehaviour {

    public float fillSpeed;

    [SerializeField] float facingAngle = -0.95f;
    [SerializeField] private GameObject _particleSystemPrefab;
    List<ShipController> _shipsInRange = new List<ShipController> ();

    private List<GameObject> _spawnedParticleSystem = new List<GameObject>();
    private List<ShipController> _attachedShips = new List<ShipController>();

    AudioSource _audioSource;

    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        bool anyShips = false;
        foreach (ShipController ship in _shipsInRange) {
            anyShips = true;
            // Check if the ship is facing us
            if (ship != null && IsFacingSun(ship)) {
                // Fill it's boost
                ship.RefillBoost(fillSpeed * Time.deltaTime);

                SpawnParticlesIfNeeded(ship);

                if(!_audioSource.isPlaying) {
                    _audioSource.Play();
                }
            }
            else
            {
                DestroyParticlesIfNeeded(ship);

                CheckSound();
            }
        }

        if(!anyShips) {
            CheckSound();
        }
    }

    bool IsFacingSun(ShipController ship) {
        bool _isFacing = false;

        // Get vector from ship to sun
        Vector3 toSun = transform.position - ship.transform.position;
        Vector3 shipUp = ship.transform.up;

        Vector3.OrthoNormalize(ref toSun, ref shipUp);

        // Check it's angle with the facing of the ship
        float angle = Vector3.Angle(ship.transform.forward, toSun);

        if (angle <= facingAngle) {
            _isFacing = true;
        }

        return _isFacing;
    }

    private void SpawnParticlesIfNeeded(ShipController ship)
    {
        if (_attachedShips.Contains(ship) == false)
        {
            _attachedShips.Add(ship);
            GameObject newParticles = Instantiate(_particleSystemPrefab, transform);
            newParticles.name = ship.name + " particles";
            newParticles.GetComponent<ParticleSystem>().Play();
            newParticles.GetComponent<ParticleAttraction>().Begin(ship.ShipModel);
            _spawnedParticleSystem.Add(newParticles);
        }

    }

    private void DestroyParticlesIfNeeded(ShipController ship)
    {
        if (_attachedShips.Contains(ship))
        {
            int ind = _attachedShips.IndexOf(ship);
            _attachedShips.Remove(ship);
            GameObject particleSystem = _spawnedParticleSystem[ind];
            _spawnedParticleSystem.RemoveAt(ind);
            particleSystem.GetComponent<ParticleSystem>().Stop();
            Destroy(particleSystem);
        }
    }

    void OnTriggerEnter(Collider other) {
        //Debug.Log("Trigger enter sun.");
        if(other.tag == "Ship") {
            _shipsInRange.Add(other.GetComponentInParent<ShipController>());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Ship") {
            var ship = other.GetComponentInParent<ShipController>();
            _shipsInRange.Remove(ship);
            DestroyParticlesIfNeeded(ship);
        }
    }

    void CheckSound() {
        if (_audioSource.isPlaying) {
            StartCoroutine(AudioFadeOut.FadeOut(_audioSource, 0.1f));
        }
    }
}
