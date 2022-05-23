using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefManager : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        // TODO: only run this on new game start
        InitializeAllPrefs();
    }

    public void InitializeAllPrefs() {
        PlayerPrefs.SetInt("Lockbox_0", 0);
        PlayerPrefs.SetInt("KnockLock_0", 0);
        PlayerPrefs.SetInt("SearchLock_0", 0);
    }

}
