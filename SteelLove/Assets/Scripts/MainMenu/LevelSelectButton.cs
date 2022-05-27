using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _levelName;
    [SerializeField] private Button _selectTrackButton;

    [Header("Listening To")]
    [SerializeField] private GameSceneEventChannelSO _onLevelSelected;

    private GameSceneSO _level;

    public void Init(GameSceneSO level)
    {
        _level = level;

        _levelName.text = _level.levelName;

        _selectTrackButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        _onLevelSelected.RaiseEvent(_level);
    }
}
