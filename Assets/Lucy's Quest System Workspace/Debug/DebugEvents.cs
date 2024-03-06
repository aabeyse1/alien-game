// Author(s): Lucy Rubin
using UnityEngine;
using System;

public class DebugEvents
{
 public event Action onDebugPressed;
    public void DebugPressed()
    {
        if (onDebugPressed != null) 
        {
            onDebugPressed();
        }
    }
}