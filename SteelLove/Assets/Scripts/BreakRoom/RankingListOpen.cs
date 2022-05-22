using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingListOpen : Interactable {

    [SerializeField] GameObject _rankingList;
    PlayerController _player;

    void Awake() {
        _player = FindObjectOfType<PlayerController>();
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }


    public override void Interact() {
        base.Interact();

        _player.Freeze();
        _rankingList.SetActive(true);
    }
}
