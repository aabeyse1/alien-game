using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))] // require this component to be attatched to the same gameobject that this script is attatched to 
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private string questId;
    private QuestState currentQuestState;
    private bool playerIsNear = false;
    
    private QuestIcon questIcon;

    private void Awake() {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable() {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

     private void OnDisable() {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;

    }

    private void SubmitPressed() {
       
        if (!playerIsNear) {
            Debug.Log("not near");
            return;
        }
        Debug.Log("near enough");
        // start or finish a quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint) {
            GameEventsManager.instance.questEvents.StartQuest(questId);
            Debug.Log("Start");
        } else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint) {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
            Debug.Log("Finish");
        }
        Debug.Log(currentQuestState);
    }

    private void QuestStateChange(Quest quest) {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId)) {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.CompareTag("Player")) {
            Debug.Log("enter");
            playerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider) {
         if (otherCollider.CompareTag("Player")) {
            playerIsNear = false;
            Debug.Log("exit");
        }
    }
   
}
