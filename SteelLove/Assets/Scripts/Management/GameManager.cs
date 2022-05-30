using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public struct CharacterRank {
    public string charaName;
    public int points;
}

public class GameManager : MonoBehaviour {

    int nextRace = 1; // What race is next to play
    public int NextRace { get => nextRace; }

    // List of characters sorted by their points
    [SerializeField] List<CharacterSO> _characterList = new List<CharacterSO>();
    public List<CharacterSO> CharacterList { get => _characterList; }


    // Singleton
    public static GameManager instance;
    bool _alive;

    void Awake() {
        SingletonCheck();

        if (_alive) {
            // Do initial setup stuff
            StartGameInitialize();
        }
    }

    public void StartGameInitialize() {
        // Player started game from main menu, so reset all data

        // Character AI settings
        List<int> aiDifficulties = new List<int>() {
            0, 1, 2, 2, 3, 3, 4
        };
        int rand = 0;
        foreach (CharacterSO character in _characterList) {
            rand = Random.Range(0, aiDifficulties.Count);
            character.difficulty = (AI_DIFFICULTY)aiDifficulties[rand];
            aiDifficulties.RemoveAt(rand);

            character.seasonPoints = 0;
            character.sabotaged = false;
        }
    }

    public int RankCompare(CharacterSO rank1, CharacterSO rank2) {
        if(rank1.seasonPoints > rank2.seasonPoints) {
            return 1;
        } else if(rank1.seasonPoints < rank2.seasonPoints) {
            return -1;
        } else {
            return 0;
        }
    }

    public void SortRankList() {
        _characterList.Sort(RankCompare);
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
        for(i = 0; i < _characterList.Count; ++i) {
            if(_characterList[i].name.Contains(charaName)) {
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

    [YarnCommand("sabotage")]
    public static void Sabotage(string name) {
        foreach(CharacterSO character in instance._characterList) {
            if (character.name.Contains(name)) {
                character.sabotaged = true;
            }
        }
    }
}
