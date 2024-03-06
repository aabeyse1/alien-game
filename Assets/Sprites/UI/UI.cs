// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objective : MonoBehaviour
{

    public GameObject subObjectiveUI;
    public GameObject subObjectiveDropDownButton;

    public TMP_Text objectiveDisplayText;
    
    private void Awake() {
        objectiveDisplayText.SetText("No active quests");
    }
    // Listen to events
    private void OnEnable() {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    // Stop listening to events
    private void OnDisable() {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;

    }

    private void QuestStateChange(Quest quest) {
        QuestState currentQuestState = quest.state;
        // show this quest name as current quest
        if (currentQuestState == QuestState.IN_PROGRESS || currentQuestState == QuestState.CAN_FINISH) {
            objectiveDisplayText.SetText(quest.info.displayName);
        } else {
             objectiveDisplayText.SetText("No active quests");
        }
        
    }
    public void ToggleObjectiveUI()
    {
        subObjectiveUI.SetActive(!subObjectiveUI.activeSelf);
        // subObjectiveDropDownButton.transform.Rotate(180,0,0); 
        StartCoroutine(RotateDropDownButton());
    }

    // Rotate the subobjective dropdown button 180 degrees
    private IEnumerator RotateDropDownButton()
    {
        int angleStep = 20;
        for (int angle = 0; angle < 180; angle += angleStep)
        {
            subObjectiveDropDownButton.transform.Rotate(0, 0, angleStep);
            yield return new WaitForSeconds(.02f);
        }
    }
}
