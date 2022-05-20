using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [Header("Broadcasting On")]
    [SerializeField] private VoidEventChannelSO _onCrossedActiveCheckpoint = default;

    MeshRenderer _mesh;

    Material _baseMaterial;
    Material _activeMaterial;
    Material _passedMaterial;

    [SerializeField] bool _isActive; // If this checkpoint is the next one the player needs to hit
    public bool _passed;

    void Awake() {
        _mesh = GetComponentInChildren<MeshRenderer>();

        _baseMaterial = _mesh.material;
        _activeMaterial = Resources.Load<Material>("Materials/Checkpoint_Active");
        _passedMaterial = Resources.Load<Material>("Materials/Checkpoint_Passed");
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Ship") {
            Debug.Log("Player hit checkpoint");

            // The player has hit this checkpoint
            Hit();
        }
    }

    void Hit() {
        if(_isActive) {
            // Activate the next checkpoint
            _onCrossedActiveCheckpoint.RaiseEvent();

            _passed = true;
            _isActive = false;

            // Change material to green
            _mesh.material = _passedMaterial;
        }
    }

    public void Reset() {
        _isActive = false;
        _passed = false;
        _mesh.material = _baseMaterial;
    }

    public void Activate() {
        _isActive = true;
        _mesh.material = _activeMaterial;
    }
}
