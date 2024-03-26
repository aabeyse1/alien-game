// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBagQuestStep : QuestStep // inherit from QuestStep
{
    private int logsCollected = 0;
    private int logsToComplete = 5; 

    private void OnEnable() { // this built in method is called when an object becomes enabled and active
        GameEventsManager.instance.pickUpEvents.onLogCollected += LogCollected; // when the event goes off, call that method
        UpdateState();
    }

    private void OnDisable() {
        GameEventsManager.instance.pickUpEvents.onLogCollected -= LogCollected;
    }

    private void LogCollected() {
        
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
        // ChangeState(state); // TODO: uncomment this
    }

    protected override void SetQuestStepState(string state) {
        this.logsCollected = System.Int32.Parse(state); // TODO - could wrap this in a try-catch block
        UpdateState();
    }
}
