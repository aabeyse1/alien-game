// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour // the class is abstract because it's meant to be inherited by another class. It will not be used on its own
{

    private bool isFinished = false;
    private string questId; // the quest that this quest step is attatched to

    private int stepIndex;

    public void InitializeQuestStep(string questId, int stepIndex, string questStepState) {
        this.questId = questId;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != "") {
            SetQuestStepState(questStepState);
        }

        Debug.Log("finished initializing quest step: " + questId);
    }


    protected void FinishQuestStep() {
        if (!isFinished) {
            isFinished = true;
            GameEventsManager.instance.questEvents.AdvanceQuest(questId);
            Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState) {
        Debug.Log("QuestStep ChangeState = " + questId);
        GameEventsManager.instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
        
    }

    protected abstract void SetQuestStepState(string state);
   
}
