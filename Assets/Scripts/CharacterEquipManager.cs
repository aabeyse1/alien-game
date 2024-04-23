using UnityEngine;

public class CharacterEquipManager : MonoBehaviour
{
    private InventorySlot currentEquippedSlot;
    private GameObject lightObject;
    private void Start() {
        GameObject character = GameObject.FindGameObjectsWithTag("Player")[0];
        lightObject = character.transform.Find("Light 2D").gameObject;
    }

    private void OnEnable() {
         GameEventsManager.instance.pickUpEvents.onItemPickedUp += ItemPickedUp;
    }

    private void OnDisable() {
         GameEventsManager.instance.pickUpEvents.onItemPickedUp -= ItemPickedUp;
    }

    private void ItemPickedUp(Item item) {
        if (currentEquippedSlot == null) {
            // TODO: Equip the item that was just picked up 
        }
    }
    
    public void UpdateEquippedItem(InventorySlot slot, Item item)
    {
        if (currentEquippedSlot != null && currentEquippedSlot != slot)
        {
            currentEquippedSlot.SetEquipped(false);
        }

        currentEquippedSlot = item != null ? slot : null;
        
        if (GetEquippedItemName() == "Torch") {
            // emit light if torch is equipped
            lightObject.SetActive(true);
        } else {
            lightObject.SetActive(false);
        }
    }

    public void SetEquipped(InventorySlot slot, bool isEquipped)
    {
        if (isEquipped)
        {
            if (currentEquippedSlot != null && currentEquippedSlot != slot)
            {
                currentEquippedSlot.SetEquipped(false);
            }
            currentEquippedSlot = slot;
        }
        else if (currentEquippedSlot == slot)
        {
            currentEquippedSlot = null;
        }
       
       
    }

    public string GetEquippedItemName() {
        if (currentEquippedSlot == null) {
            return null;
        }
        return currentEquippedSlot.GetItemName();
    }
}
