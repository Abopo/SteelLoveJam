using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardScore : MonoBehaviour {

    public TextMeshProUGUI rankText;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI timeText;


    // Start is called before the first frame update
    void Start() {
        rankText.text = "";
        playerName.text = "";
        timeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int rank, string member_id, int time) {
        rankText.text = rank + ")";
        playerName.text = member_id;

        int minutes = (int)Mathf.Floor(time / 60);
        int seconds = time - minutes * 60;
        timeText.text = minutes.ToString() + ":" + (seconds < 10 ? "0"+seconds.ToString() : seconds.ToString());
    }
}
