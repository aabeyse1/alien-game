// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Image = UnityEngine.UI.Image; // from https://stackoverflow.com/questions/64595434/how-do-i-change-the-source-image-of-a-unity-image-component
using Yarn.Unity;

public class QuestHintUI : MonoBehaviour
{

    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;

    

    [SerializeField] GameObject panel;

   

    private string currentRecipeName = "";




    
    private void OnEnable() {
        
        GameEventsManager.instance.pickUpEvents.onItemCrafted += ItemCrafted;

    }

    private void OnDisable() {
        GameEventsManager.instance.pickUpEvents.onItemCrafted -= ItemCrafted;
    }

    private void ShowQuestHint() {
        panel.SetActive(true);
        
    }

    private void HideQuestHint() {
        panel.SetActive(false);
    }
    
    [YarnCommand("SetSlotItems")]
    public void SetSlotItems(string item1Name, string item2Name, string recipeName) {
      
        
        Item[] allItemSOs = Resources.LoadAll<Item>("ItemSO");
        Sprite item1 = null;
        Sprite item2 = null;
        foreach (Item item in allItemSOs) {
            if (item.itemName == item1Name) {
                item1 = item.itemSprite;
            } 
            if (item.itemName == item2Name) {
                item2 = item.itemSprite;
            }
        }

        if (item1 != null && item2 != null) {
            slot1.GetComponent<Image>().sprite = item1;
            slot2.GetComponent<Image>().sprite = item2;
            
        } else {
            Debug.LogError("QuestHintUI: Item sprite image not found.");
        }

        currentRecipeName = recipeName;
        ShowQuestHint();
    }

    private void ItemCrafted(string itemName) {
        if (itemName == currentRecipeName) {
            HideQuestHint();
            if (itemName == "RakeRake") {
                SetSlotItems("IceSkate", "Pipe", "Axe");
            }
        }

        
    }
}
