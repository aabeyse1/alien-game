using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinsQuestStep : QuestStep // inherit from QuestStep
{
    private int coinsCollected = 0;
    private int coinsToComplete = 5; 

    // private void OnEnable() { // this built in method is called when an object becomes enabled and active
    //     GameEventsManager.instance.miscEvents.onCoinCollected += CoinCollected; // when the event goes off, call that method
    // }

    // private void OnDisable() {
    //     GameEventsManager.instance.miscEvents.onCoinCollected -= CoinCollected;
    // }

    private void CoinCollected() {
        if (coinsCollected < coinsToComplete) {
            coinsCollected++;
            UpdateState();
        }
        if (coinsCollected >= coinsToComplete) {
            FinishQuestStep();
        }
    }

    private void UpdateState() {
        string state = coinsCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state) {
        this.coinsCollected = System.Int32.Parse(state); // TODO - could wrap this in a try-catch block
        UpdateState();
    }
}
