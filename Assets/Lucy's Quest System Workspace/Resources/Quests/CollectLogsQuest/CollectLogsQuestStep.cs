// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectLogsQuestStep : QuestStep // inherit from QuestStep
{
    private int logsCollected = 0;
    private int logsToComplete = 5; 

    private void OnEnable() { // this built in method is called when an object becomes enabled and active
        GameEventsManager.instance.pickUpEvents.onLogCollected += LogCollected; // when the event goes off, call that method
        GameEventsManager.instance.playerEvents.onPlayerAreaChange += PlayerAreaChanged;

        UpdateState();
    }

    private void OnDisable() {
        GameEventsManager.instance.pickUpEvents.onLogCollected -= LogCollected;
        GameEventsManager.instance.playerEvents.onPlayerAreaChange -= PlayerAreaChanged;

    }

    private void PlayerAreaChanged(string newArea) {
        // Handles ending this log quest when the house has been exited after talking to the southern guy
        // if (newArea == "ExitHouse") {
        //     Quest currentQuest = QuestManager.instance.currentQuest;
        //     if (currentQuest.info.id == "CollectLogsQuest") {
        //         FinishQuestStep();
        //         GameEventsManager.instance.questEvents.FinishQuest(currentQuest.info.id);
        //         GameEventsManager.instance.questEvents.StartQuest("CraftBagQuest");
        //     }
        // }
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
