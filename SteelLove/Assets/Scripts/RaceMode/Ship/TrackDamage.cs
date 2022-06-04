using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrackDamage : MonoBehaviour
{
    [SerializeField] private float _raycastDist;
    [SerializeField] private float _outsideTrackFullDamage;
    [SerializeField] private float _timeToFullDamage;

    [SerializeField] private ParticleSystem _offTrackDamageParticles;

    [SerializeField] private GameObjectEventChannelSO _onShipFinishedRace = default;

    private float _currOutTrackDamage;
    private float _currOutTrackTimer;

    bool _finishedRace = false;

    ShipController _ship;
    ShipAudio _shipAudio;

    private void Start() {
        _ship = GetComponent<ShipController>();
        _shipAudio = GetComponentInChildren<ShipAudio>();
    }

    private void OnEnable() {
        _onShipFinishedRace.OnEventRaised += OnFinishedRace;
    }

    private void OnDisable() {
        _onShipFinishedRace.OnEventRaised -= OnFinishedRace;
    }

    private void FixedUpdate()
    {
        if (IsOffTrack() && !_finishedRace && _ship.Health > 0)
        {
            _currOutTrackDamage = Mathf.Lerp(0, _outsideTrackFullDamage, _currOutTrackTimer / _timeToFullDamage);

            _currOutTrackTimer += Time.deltaTime;

            DealDamage();

            if (_shipAudio != null) {
                _shipAudio.OffTrackAlarm();
            }
        }
        else
        {
            _currOutTrackTimer = 0f;
            _offTrackDamageParticles.Stop();
        }
    }

    public bool IsOffTrack()
    {
        RaycastHit hit;
        int mask = LayerMask.GetMask("TrackFloor");
        Ray rayForCast = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(rayForCast, out hit, _raycastDist, mask))
        {
            return false;
        }

        return true;
    }

    private void DealDamage()
    {
        // TODO: 
        _ship.ChangeHealth(-_currOutTrackDamage);
        if (_offTrackDamageParticles.isPlaying == false)
        {
            _offTrackDamageParticles.Play();
        }
    }

    private void OnFinishedRace(GameObject shipObj) {
        if (shipObj == gameObject) {
            _finishedRace = true;
        }
    }
}
