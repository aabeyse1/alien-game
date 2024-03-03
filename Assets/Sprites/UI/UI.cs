using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{

    public GameObject subObjectiveUI; 
    public void ToggleObjectiveUI() {
        subObjectiveUI.SetActive(!subObjectiveUI.activeSelf);
    }
}
