using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JebOShenny : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void SetDialogue() {
        // Race 1
        race1.Add("So, you've joined the race as well eh Ziv?");
        race1.Add("I don't blame ya. It's certainly hard to resist that *mmm* prize.");

        // Race 2
        race2[(int)STATE.TOP3] = new List<string>();
        race2[(int)STATE.TOP3].Add("I'm one step closer to that which I long for...");

        race2[(int)STATE.MID3] = new List<string>();
        race2[(int)STATE.MID3].Add("Mmm... not a great start, but nothing I can't handle.");
        race2[(int)STATE.MID3].Add("I've got some tricks up my sleeve for the next race heh heh heh.");

        race2[(int)STATE.BOT3] = new List<string>();
        race2[(int)STATE.BOT3].Add("No...no... I will never feel their love if I continue to lose like this...");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
