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

    public event Action<string> onItemCrafted;
    public void ItemCrafted(string itemName) 
    {
        if (onItemCrafted != null) 
        {
            onItemCrafted(itemName);
        }
    }

    
}
