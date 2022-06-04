using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using LootLocker.Requests;

public struct CharacterRank {
    public string charaName;
    public int points;
}

public class GameManager : MonoBehaviour {

    [SerializeField] int nextRace = 1; // What race is next to play
    public int NextRace { get => nextRace; }

    // List of characters sorted by their points
    [SerializeField] List<CharacterSO> _characterList = new List<CharacterSO>();
    public List<CharacterSO> CharacterList { get => _characterList; }

    public InMemoryVariableStorage yarnMemory;

    GameAudio _gameAudio;
    public GameAudio GameAudio => _gameAudio;

    public bool easyMode;

    // Singleton
    public static GameManager instance;
    bool _alive;

    void Awake() {
        SingletonCheck();

        if (_alive) {
            // Do initial setup stuff
            StartGameInitialize();

            yarnMemory = GetComponentInChildren<InMemoryVariableStorage>();
            _gameAudio = GetComponentInChildren<GameAudio>();
        }
    }

    public void StartGameInitialize() {
        // Player started game from main menu, so reset all data
        nextRace = 1;

        // Character AI settings
        List<int> aiDifficulties;
        if (easyMode)
        {
            aiDifficulties = new List<int>() {
                0, 0, 1, 1, 2, 2, 2, 3
            };
        }
        else
        {
            aiDifficulties = new List<int>() {
                0, 1, 1, 2, 2, 3, 3, 4
            };
        }
        int rand = 0;
        foreach (CharacterSO character in _characterList) {
            rand = Random.Range(0, aiDifficulties.Count);
            character.difficulty = (AI_DIFFICULTY)aiDifficulties[rand];
            aiDifficulties.RemoveAt(rand);

            character.seasonPoints = 0;
            character.sabotaged = false;

            character.upgrades = 0;
        }

        GetComponent<PlayerPrefManager>().InitializeStoryPrefs();
    }

    public int RankCompare(CharacterSO rank1, CharacterSO rank2) {
        if(rank1.seasonPoints > rank2.seasonPoints) {
            return -1;
        } else if(rank1.seasonPoints < rank2.seasonPoints) {
            return 1;
        } else {
            return 0;
        }
    }

    public void SortRankList() {
        _characterList.Sort(RankCompare);
    }

    public void PlaySongClip(AudioClip clip)
    {
        _gameAudio.PlaySongOnceThenReturn(clip);
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

        LootLockerSetup();

        StartCoroutine(LateStart());
    }

    void LootLockerSetup() {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success) {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
    }

    IEnumerator LateStart() {
        yield return null;
    }

    public void RaceFinished() {
        // Update next race
        nextRace++;

        // Make sure rank list is sorted
        SortRankList();
    }

    public STATE GetCharacterState(string charaName) {
        if(charaName == "Jims") {
            return STATE.NO_STATE;
        }
        
        // Make sure list is sorted
        SortRankList();

        STATE charaState = STATE.MID3;

        int i;
        for(i = 0; i < _characterList.Count; ++i) {
            if(_characterList[i].name.Contains(charaName)) {
                break;
            }
        }

        if(i <= 2) {
            charaState = STATE.TOP3;
        } else if(i <= 5) {
            charaState = STATE.MID3;
        } else {
            charaState = STATE.BOT3;
        }

        return charaState;
    }

    public int GetZivPosition() {
        int zivPos = 8;

        // Make sure list is sorted
        SortRankList();

        for (int i = 0; i < _characterList.Count; ++i) {
            if (_characterList[i].name.Contains("Ziv")) {
                zivPos = i;
                break;
            }
        }

        return zivPos;
    }

    [YarnCommand("upgrade")]
    public static void UpgradeShip() {
        // Increase Ziv's ship upgrades by 1
        int upgrades = 0;
        foreach (CharacterSO character in instance._characterList) {
            if (character.name.Contains("Ziv")) {
                character.upgrades += 1;
                upgrades = character.upgrades;
            }
        }

        FindObjectOfType<MainUI>().DisplayDialogue("Ship_Ziv_Upgrade_" + upgrades);
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
