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

    
    // [Header("Fuel Intro Quest")]
    // // private bool fuelIntroQuestCompleted = false; // true after the log quest in the first chunk has been started
    // public string collectLogsQuestId = "CollectLogsQuest";
    // public string craftBagQuestId = "CraftBagQuest";

    // public string fuelQuestDisplayText = "Collect Fuel";
    // public string fuelQuestDescriptionText = "Collect fuel so you can continue your mission";






    private void Awake()
    {
        objectiveDisplayText.SetText("No active quests");

        // if (!fuelIntroQuestCompleted)
        // {
        //     objectiveDisplayText.SetText(fuelQuestDisplayText);
        //     objectiveDescriptionText.SetText(fuelQuestDescriptionText);
        // }
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
        Debug.Log("quest state change = " + quest.info.id);
        
            Debug.Log("UI quest id = " + quest.info.id + " state = " + quest.state);
            QuestState currentQuestState = quest.state;
            // show this quest name as current quest
            if (currentQuestState == QuestState.IN_PROGRESS || currentQuestState == QuestState.CAN_FINISH)
            {
                objectiveDisplayText.SetText(quest.info.displayName);
                objectiveDescriptionText.SetText(quest.info.description);
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
