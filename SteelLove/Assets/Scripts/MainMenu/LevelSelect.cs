using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private List<TrackSceneSO> _raceTracks;

    [SerializeField] private Transform _levelSelectButtonParent;
    [SerializeField] private Transform _levelPreviewParent;
    [SerializeField] private TMPro.TMP_Text _levelNameText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private GameObject _levelSelectButtonPrefab;

    [Header("Listening To")]
    [SerializeField] private TrackSceneEventChannelSO _onTrackSelected;

    private GameSceneSO _selectedTrack;
    private GameObject _curPreview;

    private void OnEnable()
    {
        _onTrackSelected.OnEventRaised += SelectTrack;
    }

    private void OnDisable()
    {
        _onTrackSelected.OnEventRaised -= SelectTrack;
    }

    private void Start()
    {
        foreach(var track in _raceTracks)
        {
            var buttonObj = Instantiate(_levelSelectButtonPrefab, _levelSelectButtonParent);
            LevelSelectButton levelSelectButton = buttonObj.GetComponent<LevelSelectButton>();
            levelSelectButton.Init(track);
        }

        _confirmButton.gameObject.SetActive(false);
        _confirmButton.onClick.AddListener(ConfirmTrack);
    }

    public void SelectTrack(TrackSceneSO track)
    {
        _selectedTrack = track;

        _levelNameText.text = track.levelName;

        if(_curPreview != null)
        {
            Destroy(_curPreview);
            _curPreview = null;
        }

        _curPreview = Instantiate(track.trackPreview, _levelPreviewParent);

        _confirmButton.gameObject.SetActive(true);
    }

    public void ConfirmTrack()
    {
        _selectedTrack.sceneReference.LoadSceneAsync(LoadSceneMode.Single, true);
    }
}
