using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionEffects : MonoBehaviour
{
    [SerializeField] private float _speedRequirement;

    [SerializeField] private ParticleSystem _collisionParticles;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_rigidbody.velocity.magnitude > _speedRequirement)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(_collisionParticles, pos, rot);
        }
    }
}
