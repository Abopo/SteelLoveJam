using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    [Header("Listening To")]
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;

    public int CheckpointNumber => _checkpointNumber;
    private int _checkpointNumber;

    private MeshRenderer _mesh;

    private Material _baseMaterial;
    private Material _activeMaterial;
    private Material _passedMaterial;

    void Awake() {
        _mesh = GetComponentInChildren<MeshRenderer>();

        _baseMaterial = _mesh.material;
        _activeMaterial = Resources.Load<Material>("Materials/Checkpoint_Active");
        _passedMaterial = Resources.Load<Material>("Materials/Checkpoint_Passed");
    }

    private void OnEnable()
    {
        _onCrossedNextCheckpoint.OnEventRaised += CheckForMatChange;
    }

    private void OnDisable()
    {
        _onCrossedNextCheckpoint.OnEventRaised -= CheckForMatChange;
    }

    public void Init(int checkpointNumber)
    {
        _checkpointNumber = checkpointNumber;
    }

    private void CheckForMatChange(GameObject shipObj)
    {
        
        if (shipObj.GetComponent<PlayerShipSetup>() != null)
        {
            var checkPointTracker = shipObj.GetComponent<CheckpointTracker>();
            if (checkPointTracker.LastPassedCheckpoint == _checkpointNumber)
            {
                _mesh.material = _passedMaterial;
            }
            else if(checkPointTracker.LastPassedCheckpoint == _checkpointNumber -1)
            {
                _mesh.material = _activeMaterial;
            }
        }
    }

    public void Reset() {
        if(_checkpointNumber == 0)
        {
            _mesh.material = _activeMaterial;
        }
        else
        {
            _mesh.material = _baseMaterial;
        }
    }

    public void Activate() {
        _mesh.material = _activeMaterial;
    }
}
