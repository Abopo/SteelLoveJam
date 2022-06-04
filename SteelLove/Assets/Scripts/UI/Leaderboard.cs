using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour {

    public int ID;
    int MaxScores = 5;
    public LeaderboardScore[] Entries;

    // Start is called before the first frame update
    void Start() {

    }

    public void ShowScores() {
        LootLockerSDKManager.GetScoreList(ID, MaxScores, (response) => 
        {
            if (response.success) {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++) {
                    Entries[i].SetScore(scores[i].rank, scores[i].member_id, scores[i].score);
                    //Entries[i].text = scores[i].rank + "  " + scores[i].member_id + " - " + scores[i].score;
                }
            } else {
                Debug.Log("Get score failed");
            }
        });
    }

    public void ShowScores(int id) {
        // Reset old scores
        for (int i = 0; i < Entries.Length; i++) {
            Entries[i].Clear(i + 1);
        }

        LootLockerSDKManager.GetScoreList(id, MaxScores, (response) =>
        {
            if (response.success) {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++) {
                    Entries[i].SetScore(scores[i].rank, scores[i].member_id, scores[i].score);
                    //Entries[i].text = scores[i].rank + "  " + scores[i].member_id + " - " + scores[i].score;
                }
            } else {
                Debug.Log("Get score failed");
            }
        });
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
