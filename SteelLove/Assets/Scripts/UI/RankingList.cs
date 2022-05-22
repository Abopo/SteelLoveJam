using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingList : MonoBehaviour {

    [SerializeField] CharacterRankUI[] _charaRankUIs;

    [SerializeField] private InputReader _inputReader = default;

    PlayerController _playerController;

    void Awake() {

    }
    // Start is called before the first frame update
    void Start() {
        _playerController = FindObjectOfType<PlayerController>();

        // Fill out the ranking list
        for(int i = 0; i < 8; ++i) {
            _charaRankUIs[i]._characterText.text = GameManager.instance.rankingList[i].charaName;
            _charaRankUIs[i]._pointsText.text = GameManager.instance.rankingList[i].points.ToString();
        }
    }
    private void OnEnable() {
        _inputReader.InteractEvent += CloseList;
    }

    private void OnDisable() {
        _inputReader.InteractEvent -= CloseList;
    }

    void CloseList(float value) {
        // Give control back to player
        _playerController.Unfreeze();

        // Close
        gameObject.SetActive(false);
    }
}
