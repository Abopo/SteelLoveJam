using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// displays ships speed. assumes each unity unit = 10 meters (in scale with battlestar viper)
/// </summary>
public class Speedometer : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _UIText;

    [SerializeField] private Rigidbody _playersRigidBody;

    private Vector3 _previousPosition;

    // Update is called once per frame
    void Update()
    {
        if (_playersRigidBody != null)
        {
            float velocity = (float)System.Math.Round(_playersRigidBody.velocity.magnitude * 3.6f, 2);
            _UIText.text = "kph: " + velocity;
        }
    }

    public void Init(Rigidbody playersRigidBody)
    {
        _playersRigidBody = playersRigidBody;
    }
}
