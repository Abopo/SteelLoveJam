using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefManager : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        // TODO: Remove for final build
        //PlayerPrefs.SetInt("FirstTimePlaying", 0);

        //Only run first time game is opened
        if (PlayerPrefs.GetInt("FirstTimePlaying", 0) == 0) {
            InitializeFirstTimePrefs();
        }
    }

    public void InitializeStoryPrefs() {
        PlayerPrefs.SetInt("Lockbox_0", 0);
        PlayerPrefs.SetInt("KnockLock_0", 0);
        PlayerPrefs.SetInt("SearchLock_0", 0);
        PlayerPrefs.SetInt("Has_DarkOrb", 1);
    }

    void InitializeFirstTimePrefs() {
        PlayerPrefs.SetInt("Story Track 1 BestTime", 10000);
        PlayerPrefs.SetInt("Story Track 2 BestTime", 10000);
        PlayerPrefs.SetInt("Story Track 3 BestTime", 10000);
        PlayerPrefs.SetInt("Story Track 4 BestTime", 10000);

        PlayerPrefs.SetInt("DarkShippieDues", 1);
        PlayerPrefs.SetInt("Has_DarkOrb", 1);
    }
}
