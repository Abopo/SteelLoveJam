using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LeaderboardSubmission : MonoBehaviour {

    [SerializeField] TMP_InputField MemberID;
    public int leaderboardID; 

    // Start is called before the first frame update
    void Start() {
        
    }

    public void SubmitScore() {
        if(MemberID.text == "") {
            Debug.Log("No Member ID");
            return;
        }

        int time = (int)FindObjectOfType<TimeTracker>().TotalTime;
        LootLockerSDKManager.SubmitScore(MemberID.text, time, leaderboardID, (response) =>
        {
            if (response.success) {
                Debug.Log("Score submitted");
            } else {
                Debug.Log("Score submission failed");
            }
        });
    }


}
