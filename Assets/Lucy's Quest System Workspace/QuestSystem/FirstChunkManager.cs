using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstChunkManager : MonoBehaviour
{

    [SerializeField] private QuestInfoSO questInfoForFirstQuest;

    void Start()
    {
       
        
    }

    // void OnEnable() 
    // {
    //     GameEventsManager.instance.questEvents.onDoneLoadingQuests += DoneLoadingQuests;
    // }

    // void OnDisable() 
    // {
    //     GameEventsManager.instance.questEvents.onDoneLoadingQuests -= DoneLoadingQuests;
    // }

    // void DoneLoadingQuests() {
    //     string questId = questInfoForFirstQuest.id; 
    //     GameEventsManager.instance.questEvents.StartQuest(questId);
    // }
}
