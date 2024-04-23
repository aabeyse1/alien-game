// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellArea : MonoBehaviour
{

    private void OnEnable() {
        GameEventsManager.instance.playerEvents.onPlayerAreaChange += PlayerAreaChanged;
    }

    private void OnDisable() {
        GameEventsManager.instance.playerEvents.onPlayerAreaChange -= PlayerAreaChanged;
    }

    private void PlayerAreaChanged(string areaName) {
        if (areaName == "EnterWell") {
            if (!InventoryManager.isItemInInventory("Torch")) {
                    DialogueManager.instance.RunDialogueNode("Too_Dark");    
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
