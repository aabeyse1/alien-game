// Author(s): Lucy Rubin
// This Quest System is based off of quest-system by shapedbyrainstudios: https://github.com/shapedbyrainstudios/quest-system

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // we will want to serialize this when we save data
public class QuestStepState
{
    public string state;

    public QuestStepState(string state)
    {
        this.state = state;
    }

    public QuestStepState()
    {
        this.state = "";
    }
}
