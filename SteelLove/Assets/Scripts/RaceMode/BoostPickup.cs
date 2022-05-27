using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPickup : MonoBehaviour
{
    [SerializeField] private float _boostRecovery;

    [Header("Listening To")]
    [SerializeField] private GameObjectEventChannelSO _onLapFinished = default;

    public void Start()
    {
        _onLapFinished.OnEventRaised += Reenable;
    }

    public void OnDestroy()
    {
        _onLapFinished.OnEventRaised -= Reenable;
    }

    public void Reenable(GameObject shipObj)
    {
        if (shipObj.GetComponent<PlayerShipSetup>() != null)
        {
            gameObject.SetActive(true);
        }
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
