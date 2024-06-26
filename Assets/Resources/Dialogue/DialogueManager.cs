using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
   
    [SerializeField] public DialogueRunner dialogueRunner;
    public static DialogueManager instance { get; private set;}

    private Dictionary<string, QuestPoint> questPointMap;

    [SerializeField] public QuestPoint[] allQuestPoints;
    public static bool dialogueIsRunning = false;

    public void StartDialogue()
    {
        dialogueIsRunning = true;
    }

    public void EndDialogue()
    {
        dialogueIsRunning = false;
    }

    public void Start() {
        instance = this;
        // dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        questPointMap = CreateQuestPointMap();

    }

     private void OnEnable()
    {
        // Subscribe to events
        GameEventsManager.instance.questEvents.onDoneLoadingQuests += DoneLoadingQuests;
    }

    private void OnDisable()
    {
        // Subscribe to events
        GameEventsManager.instance.questEvents.onDoneLoadingQuests -= DoneLoadingQuests;
    }

    private void DoneLoadingQuests() {
        RunDialogueNode("InitialDialogue");
    }


    public void RunDialogueNode(string conversationStartNode) {
        dialogueRunner.StartDialogue(conversationStartNode);
    }

    [YarnCommand("StartQuest")]
    public void StartQuest(string id) {  
        GameEventsManager.instance.questEvents.StartQuest(id);
            
    }

    [YarnCommand("setDialogueNode")]
    public void setDialogueNode(string questPointId, string nodeName) {
        QuestPoint questPoint = GetQuestPointById(questPointId);
        questPoint.dialogueNodeForPoint = nodeName;
    }

    private Dictionary<string, QuestPoint> CreateQuestPointMap()
    {   
        // create the map
        Dictionary<string, QuestPoint> idToQuestPointMap = new Dictionary<string, QuestPoint>();
        foreach (QuestPoint questPoint in allQuestPoints)
        {
            if (idToQuestPointMap.ContainsKey(questPoint.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest point map: " + questPoint.id);
            }
            idToQuestPointMap.Add(questPoint.id, questPoint);
        }

        return idToQuestPointMap;

    }

    public QuestPoint GetQuestPointById(string id)
    {
        
        QuestPoint questPoint = questPointMap[id];
        if (questPoint == null)
        {
            Debug.LogError("ID not found in the Quest Point Map: " + id);
        }
        return questPoint;
    }
}
