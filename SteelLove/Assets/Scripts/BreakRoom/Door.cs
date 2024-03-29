﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] Door _pairedDoor;
    public Room room;

    // Start is called before the first frame update
    void Start() {
        room = GetComponentInParent<Room>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            // Warp the player to our paired door
            other.transform.parent.position = new Vector3(_pairedDoor.transform.position.x,
                                                           _pairedDoor.transform.position.y,
                                                           other.transform.parent.position.z);
            // Activate the paired room
            _pairedDoor.room.Enter();
            // Close our room
            room.Exit();
        }
    }
}
