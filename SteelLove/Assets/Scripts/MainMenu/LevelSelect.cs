using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private SceneManagerSO _sceneManager;
    [SerializeField] private List<TrackSceneSO> _raceTracks;
    [SerializeField] private TrackSceneSO _dsdTrack;

    [SerializeField] private Transform _levelSelectButtonParent;
    [SerializeField] private Transform _levelPreviewParent;
    [SerializeField] private TMPro.TMP_Text _levelNameText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private GameObject _levelSelectButtonPrefab;

    [SerializeField] private GameSceneSO _menuScene;
    [SerializeField] private RaceStateSO _raceStateSO;

    [Header("Listening To")]
    [SerializeField] private TrackSceneEventChannelSO _onTrackSelected;

    private GameSceneSO _selectedTrack;
    private GameObject _curPreview;

    Leaderboard _leaderboard;

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
        if(PlayerPrefs.GetInt("DarkShippieDues", 0) > 0) {
            _raceTracks.Add(_dsdTrack);
        }

        foreach(var track in _raceTracks)
        {
            var buttonObj = Instantiate(_levelSelectButtonPrefab, _levelSelectButtonParent);
            LevelSelectButton levelSelectButton = buttonObj.GetComponent<LevelSelectButton>();
            levelSelectButton.Init(track);
        }

        _confirmButton.gameObject.SetActive(false);
        _confirmButton.onClick.AddListener(ConfirmTrack);

        _leaderboard = GetComponentInChildren<Leaderboard>();
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

        // Load leaderboard scores for the track
        if (track.trackLeaderboardID != 0) {
            _leaderboard.Show();
            _leaderboard.ShowScores(track.trackLeaderboardID);
        } else {
            _leaderboard.Hide();
        }
    }

    public void ConfirmTrack()
    {
        _sceneManager.LoadScene(_selectedTrack);
    }

}
