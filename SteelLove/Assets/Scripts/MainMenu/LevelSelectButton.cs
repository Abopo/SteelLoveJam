using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _levelName;
    [SerializeField] private Button _selectTrackButton;

    [Header("Listening To")]
    [SerializeField] private TrackSceneEventChannelSO _onTrackSelected;

    private TrackSceneSO _level;

    public void Init(TrackSceneSO level)
    {
        _level = level;

        _levelName.text = _level.levelName;

        _selectTrackButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        _onTrackSelected.RaiseEvent(_level);
    }
}
