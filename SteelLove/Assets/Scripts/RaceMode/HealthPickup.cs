using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float _healthRecovery;

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
        ship.ChangeHealth(_healthRecovery);
        gameObject.SetActive(false);
    }
}
