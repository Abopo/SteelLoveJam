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

    private float _currOutTrackDamage;
    private float _currOutTrackTimer;
    private float _currInsideTrackHealing;
    private float _currInsideTrackTimer;

    private void FixedUpdate()
    {
        if (IsOffTrack())
        {
            _currInsideTrackHealing = 0;
            _currInsideTrackTimer = 0;

            _currOutTrackDamage = Mathf.Lerp(0, _outsideTrackFullDamage, _currOutTrackTimer / _timeToFullDamage);

            _currOutTrackTimer += Time.deltaTime;

            DealDamage();
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

    private bool IsOffTrack()
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
        GetComponent<ShipController>().ChangeHealth(-_currOutTrackDamage);
    }

    private void Heal()
    {
        GetComponent<ShipController>().ChangeHealth(_currInsideTrackHealing);
    }
}
