using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrackDamage : MonoBehaviour
{
    [SerializeField] private float _raycastDist;
    [SerializeField] private float _outsideTrackDamageBuildup;
    [SerializeField] private float _outsideTrackFullDamage;
    [SerializeField] private float _timeToFullDamage;
    [SerializeField] private float _insideTrackHealingBuildup;
    [SerializeField] private float _insideTrackFullHeal;
    [SerializeField] private float _timeToFullHealing;

    [SerializeField] private GameObject _centerPoint;

    [SerializeField] private ParticleSystem _offTrackDamageParticles;

    private float _currOutTrackDamage;
    private float _currOutTrackTimer;
    private float _currInsideTrackHealing;
    private float _currInsideTrackTimer;

    ShipController _ship;
    ShipAudio _shipAudio;

    private void Start() {
        _ship = GetComponent<ShipController>();
        _shipAudio = GetComponentInChildren<ShipAudio>();
    }

    private void FixedUpdate()
    {
        if (IsOffTrack())
        {
            _currInsideTrackHealing = 0;
            _currInsideTrackTimer = 0;

            _currOutTrackDamage = Mathf.Lerp(0, _outsideTrackFullDamage, _currOutTrackTimer / _timeToFullDamage);

            _currOutTrackTimer += Time.deltaTime;

            DealDamage();

            if (_shipAudio != null) {
                _shipAudio.OffTrackAlarm();
            }
        }
        else
        {
            _currOutTrackDamage = 0;
            _currOutTrackTimer = 0;

            _currInsideTrackHealing = Mathf.Lerp(0, _insideTrackFullHeal, _currInsideTrackTimer / _timeToFullHealing);

            _currInsideTrackTimer += Time.deltaTime;

            Heal();
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

    private void Heal()
    {
        _ship.ChangeHealth(_currInsideTrackHealing);
        if (_offTrackDamageParticles.isPlaying)
        {
            _offTrackDamageParticles.Stop();
        }
    }

}
