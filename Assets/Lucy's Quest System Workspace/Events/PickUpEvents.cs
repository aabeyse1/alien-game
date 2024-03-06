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

    
}
