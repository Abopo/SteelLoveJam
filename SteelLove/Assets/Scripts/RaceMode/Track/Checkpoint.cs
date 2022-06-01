using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    [SerializeField] private List<MeshRenderer> _bannerMeshFront;
    [SerializeField] private List<MeshRenderer> _bannerMeshBack;

    [Header("Listening To")]
    [SerializeField] private GameObjectEventChannelSO _onCrossedNextCheckpoint = default;

    public int CheckpointNumber => _checkpointNumber;
    private int _checkpointNumber;

    private Material _baseMaterial;
    private Material _activeMaterial;
    private Material _passedMaterial;

    AudioSource _audioSource;

    void Awake() {
        _baseMaterial = _bannerMeshFront[0].material;
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

    public void Reset()
    {
        if (_checkpointNumber == 0)
        {
            SetBannerMeshMat(_activeMaterial);
        }
        else
        {
            SetBannerMeshMat(_baseMaterial);
        }
    }

    public void Activate()
    {
        SetBannerMeshMat(_activeMaterial);
    }

    private void CheckForMatChange(GameObject shipObj)
    {
        
        if (shipObj.GetComponent<PlayerShipSetup>() != null)
        {
            var checkPointTracker = shipObj.GetComponent<CheckpointTracker>();
            if (checkPointTracker.LastPassedCheckpoint == _checkpointNumber)
            {
                SetBannerMeshMat(_passedMaterial);

                _audioSource.Play();
            }
            else if(checkPointTracker.LastPassedCheckpoint == _checkpointNumber -1)
            {
                SetBannerMeshMat(_activeMaterial);
            }
        }
    }

    private void SetBannerMeshMat(Material mat)
    {
        foreach(var banner in _bannerMeshFront)
        {
            banner.material = mat;
        }
        foreach(var banner in _bannerMeshBack)
        {
            banner.material = mat;
        }
    }
}
