// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{

    public GameObject debugUI;

    public TMP_Text currentQuestText;
    public TMP_Text finishQuestButtonText; 

    public QuestManager questManger;

    // Listen to events
    private void OnEnable()
    {
        DebugEventManager.instance.debugEvents.onDebugPressed += DebugPressed;
        GameEventsManager.instance.questEvents.onStartQuest += QuestStarted;
        GameEventsManager.instance.questEvents.onFinishQuest += QuestFinished;
    }

    private void OnDisable()
    {
        DebugEventManager.instance.debugEvents.onDebugPressed -= DebugPressed;
        GameEventsManager.instance.questEvents.onStartQuest -= QuestStarted;

    }

    // toggle the visibility of the debug panel when key is pressed (right now it is "q")
    private void DebugPressed()
    {
        Canvas canvas = debugUI.GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }

    // Display on debug panel as current quest and on the finish quest button
    private void QuestStarted(string id) {
        currentQuestText.SetText("Current Quest: " + id);
        finishQuestButtonText.SetText(id);
    }

    private void QuestFinished(string id) {
        currentQuestText.SetText("No current quest");
        finishQuestButtonText.SetText("No current quest");
    }

    // finish the quest specified by id currently being shown on the button
    public void FinishQuest() {
        string currentQuestId = finishQuestButtonText.text;
        if (questManger.GetQuestById(currentQuestId) != null) {
            GameEventsManager.instance.questEvents.FinishQuest(currentQuestId);
        }
        
    }

    // Send event that one log was collected
    public void collectOneLog() {
        GameEventsManager.instance.pickUpEvents.LogCollected();
    }

}