using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct CharacterRank {
    public string charaName;
    public int points;
}

public class GameManager : MonoBehaviour {

    int nextRace = 1; // What race is next to play
    public int NextRace { get => nextRace; }

    // List of characters sorted by their points
    public List<CharacterRank> rankingList = new List<CharacterRank>();

    // Singleton
    public static GameManager instance;
    bool _alive;

    void Awake() {
        SingletonCheck();

        if (_alive) {
            // Do initial setup stuff
            CreateRankList();
        }
    }

    void CreateRankList() {
        CharacterRank tempRank = new CharacterRank();
        tempRank.points = 0;

        tempRank.charaName = "Ziv Chromebeak";
        rankingList.Add(tempRank);
        tempRank.charaName = "Jebb O' Shenny";
        rankingList.Add(tempRank);
        tempRank.charaName = "Mrs. Frieda";
        rankingList.Add(tempRank);
        tempRank.charaName = "Charlene Chibs";
        rankingList.Add(tempRank);
        tempRank.charaName = "Damian Dubble";
        rankingList.Add(tempRank);
        tempRank.charaName = "Sumiv Warring";
        rankingList.Add(tempRank);
        tempRank.charaName = "Aldious Ripley";
        rankingList.Add(tempRank);
        tempRank.charaName = "Trik";
        rankingList.Add(tempRank);
    }

    public int RankCompare(CharacterRank rank1, CharacterRank rank2) {
        if(rank1.points > rank2.points) {
            return 1;
        } else if(rank1.points < rank2.points) {
            return -1;
        } else {
            return 0;
        }
    }

    public void SortRankList() {
        rankingList.Sort(RankCompare);
    }

    void SingletonCheck() {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        if (obj != null && obj != this.gameObject) {
            _alive = false;
            DestroyImmediate(this.gameObject);
        } else {
            instance = this;
            _alive = true;
        }
    }

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(transform.gameObject);

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return null;

        // This is the first time the game is opened so initialize stuff
        if (nextRace == 1) {
            FindObjectOfType<MainUI>().DisplayDialogue("Initialize");
        }
    }

    public void RaceFinished() {
        // Update next race
        nextRace++;

        // Load Break Room
        SceneManager.LoadScene("BreakRoom");
    }

    public STATE GetCharacterState(string charaName) {
        STATE charaState = STATE.NO_STATE;

        int i;
        for(i = 0; i < rankingList.Count; ++i) {
            if(rankingList[i].charaName.Contains(charaName)) {
                break;
            }
        }

        if(i <= 2) {
            charaState = STATE.TOP3;
        } else if(i <= 6) {
            charaState = STATE.MID3;
        } else {
            charaState = STATE.BOT3;
        }

        return charaState;
    }
}
