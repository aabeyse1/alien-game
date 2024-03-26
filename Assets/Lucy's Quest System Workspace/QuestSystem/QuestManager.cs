// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;
    private Dictionary<string, Quest> questMap;

    public Quest currentQuest {get; private set;} 

    public static QuestManager instance { get; private set; }

    // quest start requirements
    private int currentPlayerLevel = 1;  // TODO: get rid of this being hard coded

    private void Awake()
    {
        instance = this;
        questMap = CreateQuestMap();

    }

    private void OnEnable()
    {
        // Subscribe to the quest events
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;

    }

    private void OnDisable()
    {
        // Unsubscribe to the quest events
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;


        GameEventsManager.instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;

    }

    private void Start()
    {
        
        foreach (Quest quest in questMap.Values)
        {   
            // initialize any loaded quest steps
            if (quest.state == QuestState.IN_PROGRESS) {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
        
        // // Let everyone know that all of the quests have been loaded in 
        // Debug.Log("Done loading quests");
        // GameEventsManager.instance.questEvents.DoneLoadingQuests();
    }


    private void ChangeQuestState(string id, QuestState state)
    {   
        Quest quest = GetQuestById(id);
        quest.state = state;
        
        GameEventsManager.instance.questEvents.QuestStateChange(quest); // let everyone else know that the state changed
    }

    private void PlayerLevelChange(int level)
    {
        currentPlayerLevel = level;
    }

    // check if this quest can be accessed yet
    private bool CheckRequirementsMet(Quest quest) {
        // start with true and prove it to be false
        bool meetsRequirements = true;

        // check player level requirements
        if (currentPlayerLevel < quest.info.levelRequirement) {
            meetsRequirements = false;
        }
        // check quest prerequisites for completion
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites) {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED) {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update() {
        // loop through all of the quests
        foreach(Quest quest in questMap.Values) {
            // if we are now meeting the requirements, switch over to the CAN_START state
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest)) {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }
    private void StartQuest(string id)
    {
       Debug.Log("Start quest " + id);
       Quest quest = GetQuestById(id);
       quest.InstantiateCurrentQuestStep(this.transform); // instantiate the quest step under the quest manager game object
       ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
       currentQuest = quest; 
       Debug.Log("Started quest. Current quest = " + currentQuest.info.id);
    }

    private void AdvanceQuest(string id)
    {
       Quest quest = GetQuestById(id);

       // move on to next step
       quest.MoveToNextStep();

       // if there are more steps, instantiate the next one
       if (quest.CurrentStepExists()) {
            quest.InstantiateCurrentQuestStep(this.transform);
       }
       // if there are no more steps, then we've finished all of them for this quest
       else {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
       }
    }

    private void FinishQuest(string id)
    {

        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

    }

    private void ClaimRewards(Quest quest) {
        GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState) {

        Quest quest = GetQuestById(id);
        
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }
    private Dictionary<string, Quest> CreateQuestMap()
    {
        // loads all QuestInfoSo scriptable objects under the Assets/Resources/Quests folder
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        // create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }

        return idToQuestMap;

    }

    // catch errors if we try to access a quest id that doesn't exist
    public Quest GetQuestById(string id)
    {
        
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    // save data
    private void OnApplicationQuit() {
        foreach (Quest quest in questMap.Values) {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest) {
        try {
            QuestData questData = quest.GetQuestData();
            // serialze using JsonUtility, can upgrade to using Json dotnet if having issues with serializing
            string serializedData = JsonUtility.ToJson(questData);
            //TODO - using PlayerPrefs to save quests temporarily. Should change this to an actual save and load system and write to a file/cloud/etc
            PlayerPrefs.SetString(quest.info.id, serializedData);

        } catch (System.Exception e){
            Debug.LogError("Failed to save quest with id " + quest.info.id + ": " +  e);
        }
    }

    private Quest LoadQuest(QuestInfoSO questInfo) {
        Quest quest = null;
        try {
            // load quest from saved data
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState) {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            } // otherwise initialize a new quest
            else {
                quest = new Quest(questInfo);
            }
        } catch (System.Exception e) {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e );
        }
        return quest; 
    }
}
