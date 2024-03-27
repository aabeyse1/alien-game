// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBagQuestStep : QuestStep // inherit from QuestStep
{

    [SerializeField] private string backpackItemName = "Backpack";

    private void OnEnable() { 
        GameEventsManager.instance.pickUpEvents.onItemCrafted += ItemCrafted;
       
    }

    private void OnDisable() {
        GameEventsManager.instance.pickUpEvents.onItemCrafted -= ItemCrafted;

    }


    private void ItemCrafted(string itemName) {
        if (itemName == backpackItemName) {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        
    }
}
