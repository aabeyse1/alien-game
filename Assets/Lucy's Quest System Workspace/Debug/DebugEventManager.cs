// Author(s): Lucy Rubin
using System;
using UnityEngine;

public class DebugEventManager : MonoBehaviour
{
    public static DebugEventManager instance { get; private set; }

    public DebugEvents debugEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Debug Events Manager in the scene.");
        }
        instance = this;
        

        // initialize all events
        debugEvents = new DebugEvents();
       
    }
}
