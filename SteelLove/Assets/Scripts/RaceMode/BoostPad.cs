using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] private float _boostForceMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            OnHit(other.transform.parent.GetComponent<ShipController>());
        }
    }

    private void OnHit(ShipController ship)
    {
        ship.BoostPadActivate(_boostForceMultiplier);
    }
}
