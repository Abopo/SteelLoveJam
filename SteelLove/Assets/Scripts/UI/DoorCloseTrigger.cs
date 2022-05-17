using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorCloseTrigger : MonoBehaviour {

    [SerializeField] CanvasGroup _roomName;

    // Start is called before the first frame update
    void Start() {
        _roomName.alpha = 0;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _roomName.alpha = 1;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _roomName.alpha = 0;
    }
}
