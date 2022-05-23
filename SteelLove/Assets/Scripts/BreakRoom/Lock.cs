using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Interactable {

    [SerializeField] LOCKS _lockSide;

    Lockbox _parentLockbox;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        _parentLockbox = GetComponentInParent<Lockbox>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact() {
        base.Interact();

        _parentLockbox.LockPressed(_lockSide);

        // Play a little sound?
    }
}
