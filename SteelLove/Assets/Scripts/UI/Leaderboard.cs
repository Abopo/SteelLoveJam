using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using LootLocker.Admin;
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

}
