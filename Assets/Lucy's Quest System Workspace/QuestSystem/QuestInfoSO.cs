// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
   [field: SerializeField] public string id { get; private set;} // Used to reference a specific quest across the entire system. Unique to each quest

    [Header("General")]
    public string displayName; // the name that will appear in the game

    [Header("Requirements")]
    public int levelRequirement; // player level required to start this quest
    public QuestInfoSO[] questPrerequisites; // quests that need to be completed before this one can be started
    
    [Header("Steps")]
    public GameObject[] questStepPrefabs; // array of game objects that are the steps taken to complete the quest

    [Header("Rewards")]
    public int goldReward; // temporary
    public int experienceReward; // temporary

    // forces the id of a quest to be the same as the name of the scriptable object
    private void OnValidate(){ // editor-only function called when the script is loaded or value is changed in the inspector
        #if UNITY_EDITOR 
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
    
       

}
