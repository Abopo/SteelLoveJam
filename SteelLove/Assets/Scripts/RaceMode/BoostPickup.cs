﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPickup : MonoBehaviour
{
    [SerializeField] private float _boostRecovery;

    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO _onCrossedFinishLine = default;

    public void Start()
    {
        _onCrossedFinishLine.OnEventRaised += Reenable;
    }

    public void OnDestroy()
    {
        _onCrossedFinishLine.OnEventRaised -= Reenable;
    }

    public void Reenable()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship" && other.transform.parent.GetComponent<PlayerShipSetup>() != null)
        {
            OnHit(other.transform.parent.GetComponent<ShipController>());
        }
    }

    private void OnHit(ShipController ship)
    {
        ship.RefillBoost(_boostRecovery);
        gameObject.SetActive(false);
    }
}