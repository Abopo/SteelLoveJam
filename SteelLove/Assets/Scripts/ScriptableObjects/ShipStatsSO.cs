using UnityEngine;

[CreateAssetMenu(fileName = "ShipStats", menuName = "ShipStats")]
public class ShipStatsSO : DescriptionBaseSO
{
    [Header("Thruster Properties")]
    [SerializeField] public float _mainthrustForce;
    [SerializeField] public float _reverseThrustForce;
    [SerializeField] public float _horizontalThrustForce;
    [SerializeField] public float _maxSpeed;

    [Header("extreme direction change")]
    [SerializeField] public float _extremeDirChangeAngle;
    [SerializeField] public float _extremeDirChangeMult;

    [Header("Turning")]
    [SerializeField] public float _rotForce;
    [SerializeField] public float _maxRotSpeed;

    [Header("Boosting")]
    [SerializeField] public float _boostForceMultiplier;
    [SerializeField] public float _boostCost;
    [SerializeField] public float _maxSpeedBoostModifier;

    [Header("BoostPad")]
    [SerializeField] public float _boostPadNoSpeedLimitTime;

    [Header("Over Speed limit Slowdown")]
    [SerializeField] public float _overSpeedLimitSlowdownForce;

    [Header("Starting resources")]
    [SerializeField] public float _startingHealth = 100.0f;
    [SerializeField] public float _startingBoostTank = 0f;
}
