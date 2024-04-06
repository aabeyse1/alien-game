// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectLogsAfterBagQuestStep : QuestStep // inherit from QuestStep
{
    private int logsCollected = 0;
    private int logsToComplete = 5; 

    private void OnEnable() { // this built in method is called when an object becomes enabled and active
        GameEventsManager.instance.pickUpEvents.onItemPickedUp += ItemPickedUp; // when the event goes off, call that method
    }

    private void OnDisable() {
        GameEventsManager.instance.pickUpEvents.onItemPickedUp -= ItemPickedUp;
       

    }


    private void ItemPickedUp(Item item) {
        string itemName = item.name;
        if (itemName != "Log") {
            return;
        }
        if (logsCollected < logsToComplete) {
            logsCollected++;
            UpdateState();
        }
        if (logsCollected >= logsToComplete) {
            FinishQuestStep();
        }
        Debug.Log(logsCollected + "/" + logsToComplete + " logs collected");
    }

    private void UpdateState() {
        
        string state = logsCollected.ToString();
       
        ChangeState(state);
        
    }

    protected override void SetQuestStepState(string state) {
        this.logsCollected = System.Int32.Parse(state); // TODO - could wrap this in a try-catch block
        UpdateState();
    }
}
