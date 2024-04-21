using UnityEngine;

public class CharacterEquipManager : MonoBehaviour
{
    private InventorySlot currentEquippedSlot;
    
    
    public void UpdateEquippedItem(InventorySlot slot, Item item)
    {
        if (currentEquippedSlot != null && currentEquippedSlot != slot)
        {
            currentEquippedSlot.SetEquipped(false);
        }

        currentEquippedSlot = item != null ? slot : null;
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
