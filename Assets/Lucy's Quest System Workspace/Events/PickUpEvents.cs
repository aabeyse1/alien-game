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

    public event Action<string> onItemCrafted;
    public void ItemCrafted(string itemName) 
    {
        if (onItemCrafted != null) 
        {
            onItemCrafted(itemName);
        }
    }

    
}
