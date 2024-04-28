// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(CircleCollider2D))] // require this component to be attatched to the same gameobject that this script is attatched to 
public class QuestPoint : MonoBehaviour
{

    [field: SerializeField] public string id { get; private set; } // Used to reference a specific quest point across the entire system. Unique to each quest point

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Dialogue")]
    [SerializeField] private DialogueRunner dialogueRunnerForPoint;

    [SerializeField] public string dialogueNodeForPoint;

    // [SerializeField] private DialogueSO dialogueWhileQuestActive;

    // [SerializeField] private string dialogueBeforeQuestActive;

    [SerializeField] private bool runDialogueBeforeQuest = true;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private string questId;
    private QuestState currentQuestState;
    private bool playerIsNear = false;

    [SerializeField] GameObject speechBubbleObject;

    private QuestIcon questIcon;

    [SerializeField] GameObject enterTutorialObject;

    private void Awake()
    {
        if (questInfoForPoint) {
             questId = questInfoForPoint.id;
        }
       
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;

    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;

    }



    private void SubmitPressed()
    {

        if (!playerIsNear)
        {
            return;
        }

        // Tutorial won't show up anymore
        if (!TutorialManager.instance.hasTalkedToNPC)
        {
            TutorialManager.instance.hasTalkedToNPC = true;
            if (enterTutorialObject)
            {
                enterTutorialObject.SetActive(false);
            }
        }

        if (runDialogueBeforeQuest)
        {
            // Run dialogue
            DialogueManager.instance.RunDialogueNode(dialogueNodeForPoint);
        }
        else
        {
            // start or finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                Debug.Log("FINISH QUEST");
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }
        }

    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            // questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
            speechBubbleObject.SetActive(true);
            if (!TutorialManager.instance.hasTalkedToNPC && enterTutorialObject)
            {
                // if haven't poked yet, show the spacebar icon telling you how to use tools
                enterTutorialObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            speechBubbleObject.SetActive(false);
            if (enterTutorialObject)
            {
                enterTutorialObject.SetActive(false);
            }
        }
    }

    // forces the id of a quest to be the same as the name of the scriptable object
    private void OnValidate()
    { // editor-only function called when the script is loaded or value is changed in the inspector
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

}
