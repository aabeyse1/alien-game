// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFuelQuestStep : QuestStep // inherit from QuestStep
{
    // private int logsCollected = 0;
    // private int logsToComplete = 5; 

    private void OnEnable() { // this built in method is called when an object becomes enabled and active
        // GameEventsManager.instance.pickUpEvents.onLogCollected += LogCollected; // when the event goes off, call that method
        // UpdateState();
    }

    private void OnDisable() {
        // GameEventsManager.instance.pickUpEvents.onLogCollected -= LogCollected;
    }

   
    private void UpdateState() {
        // string state = logsCollected.ToString();
        // ChangeState(state); // TODO: uncomment this? 
    } 

    protected override void SetQuestStepState(string state) {
        // this.logsCollected = System.Int32.Parse(state); // TODO - could wrap this in a try-catch block
        UpdateState();
    }
}
