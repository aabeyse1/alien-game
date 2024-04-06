// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objective : MonoBehaviour
{

    [Header("Quest Dropdown")]
    public GameObject subObjectiveUI;
    public GameObject subObjectiveDropDownButton;

    [Header("Quest Text")]
    public TMP_Text objectiveDisplayText;

    public TMP_Text objectiveDescriptionText;
    public TMP_Text progressText;


    private void Awake()
    {
        objectiveDisplayText.SetText("No active quests");

    }
    // Listen to events
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;

    
    }

    // Stop listening to events
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;


    }

    private void QuestStateChange(Quest quest)
    {
            QuestState currentQuestState = quest.state;
            // show this quest name as current quest
            if (currentQuestState == QuestState.IN_PROGRESS || currentQuestState == QuestState.CAN_FINISH)
            {
                objectiveDisplayText.SetText(quest.info.displayName);
                objectiveDescriptionText.SetText(quest.info.description);

                if (currentQuestState == QuestState.IN_PROGRESS) {
                    // Quest just started
                    progressText.SetText("Progress: 0");
                }                
            }
            else
            {
                objectiveDisplayText.SetText("No active quests");
                objectiveDescriptionText.SetText("Information about this objective");
                progressText.SetText("Progress: N/A");
            }
        


    }

    private void QuestStepStateChange(string questId, int stepIndex, QuestStepState questStepState)
    {
        
            try
            {
                int numCollected = System.Int32.Parse(questStepState.state);
                progressText.SetText("Progress: " + numCollected + " collected");
            }
            catch
            {
                Debug.Log("Error in converting questStepState to int"); // TODO: handle different types of quest step states
            }
        

    }

    private void StartQuest(string id) {
            Debug.Log("Start quest UI = "+ id);
            Quest quest = QuestManager.instance.GetQuestById(id);
            QuestStateChange(quest);
        
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
