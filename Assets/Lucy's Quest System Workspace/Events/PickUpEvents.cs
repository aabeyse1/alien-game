// Author(s): Lucy Rubin
using System;

public class PickUpEvents
{
    public event Action onLogCollected;
    public void LogCollected() 
    {
        if (onLogCollected != null) 
        {
            onLogCollected();
        }
    }

    public event Action<Item> onItemPickedUp;

    public void ItemPickedUp(Item item) {
        if (onItemPickedUp != null) {
            onItemPickedUp(item);
        }
    }

    public event Action onItemDropped;

    public void ItemDropped() {
        if (onItemDropped != null) {
            onItemDropped();
        }
    }

    public event Action onItemEquipped;

    public void ItemEquipped() {
        if (onItemEquipped != null) {
            onItemEquipped();
        }
    }

    public event Action<string> onItemCrafted;
    public void ItemCrafted(string itemName) 
    {
        if (onItemCrafted != null) 
        {
            onItemCrafted(itemName);
        }
    }

    
}
