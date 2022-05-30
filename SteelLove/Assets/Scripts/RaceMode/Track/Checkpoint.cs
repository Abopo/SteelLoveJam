using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    [SerializeField] private MeshRenderer _bannerMeshFront;
    [SerializeField] private MeshRenderer _bannerMeshBack;

    [Header("Listening To")]
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;

    public int CheckpointNumber => _checkpointNumber;
    private int _checkpointNumber;

    private Material _baseMaterial;
    private Material _activeMaterial;
    private Material _passedMaterial;

    AudioSource _audioSource;

    void Awake() {
        _baseMaterial = _bannerMeshFront.material;
        _activeMaterial = Resources.Load<Material>("Materials/Checkpoint_Active");
        _passedMaterial = Resources.Load<Material>("Materials/Checkpoint_Passed");

        _audioSource = GetComponent<AudioSource>();
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
                _bannerMeshFront.material = _passedMaterial;
                _bannerMeshBack.material = _passedMaterial;

                _audioSource.Play();
            }
            else if(checkPointTracker.LastPassedCheckpoint == _checkpointNumber -1)
            {
                _bannerMeshFront.material = _activeMaterial;
                _bannerMeshBack.material = _activeMaterial;
            }
        }
    }

    public void Reset() {
        if(_checkpointNumber == 0)
        {
            _bannerMeshFront.material = _activeMaterial;
            _bannerMeshBack.material = _activeMaterial;
        }
        else
        {
            _bannerMeshFront.material = _baseMaterial;
            _bannerMeshBack.material = _baseMaterial;
        }
    }

    public void Activate() {
        _bannerMeshFront.material = _activeMaterial;
        _bannerMeshBack.material = _activeMaterial;
    }
}
