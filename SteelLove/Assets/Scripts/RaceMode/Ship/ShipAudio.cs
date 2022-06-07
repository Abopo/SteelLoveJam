using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAudio : MonoBehaviour {

    [SerializeField] AudioSource _sparksSound;
    [SerializeField] AudioSource _explosionSound;

    [SerializeField] private AudioSource _offTrackAlarm;
    private float _alarmTime = 1f;
    private float _alarmTimer = 0f;

    [SerializeField] AudioSource _boostSound;
    [SerializeField] AudioClip _deathClip;
    [SerializeField] AudioClip _menuClip;

    ShipController _ship;

    // Start is called before the first frame update
    void Start() {
        _ship = GetComponentInParent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySparks() {
        if (!_sparksSound.isPlaying) {
            _sparksSound.Play();
        }
    }

    public void PlayExplosion() {
        _explosionSound.Play();
    }

    public void OffTrackAlarm() {
        if (_offTrackAlarm != null && !_offTrackAlarm.isPlaying && _ship.Health > 0) {

            _alarmTimer += Time.deltaTime;
            if (_alarmTimer >= _alarmTime) {
                _offTrackAlarm.Play();
                _alarmTimer = 0;

                // Set alarm time based on ships health
                _alarmTime = 0.8f * (_ship.Health / 100);
            }
        }
    }

    public void PlayBoostSound() {
        _boostSound.volume = 1f;
        _boostSound.Play();
    }
    public void StopBoostSound() {
        StartCoroutine(AudioFadeOut.FadeOut(_boostSound, 1f));
    }

    public void PlayDeathClip() {
        GameManager.instance.PlaySongClipThenLoopSecondClip(_deathClip, _menuClip);
    }
}
