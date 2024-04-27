using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour 
{
    public bool hasEnteredAnArea = false;
    public bool hasChoppedTree = false;
    public bool hasDraggedItemForCrafting = false; 

    public bool hasPokedSkate = false;
    public bool hasPickedUpItem = false;

    public bool hasTalkedToNPC = false;

     public static TutorialManager instance { get; private set; }

    private void Awake() {
        instance = this;
    }

    
}
