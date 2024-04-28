// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class GetMoreHoneyQuestStep : QuestStep // inherit from QuestStep
{

public static GetMoreHoneyQuestStep instance { get; private set; }


private GameObject animationObject;


   
    private void Awake() {
        instance = this;
        animationObject = GameObject.FindGameObjectsWithTag("EarSpinAnimationPrefab")[0];
        Debug.Log("AWAKE instance = " + instance);
    }
    private void OnEnable() { 
       
       
    }

    private void OnDisable() {

    }




    protected override void SetQuestStepState(string state)
    {
        
    }

    [YarnCommand("ShowAttackOption")]
    public void ShowAttackButton() {
        GameObject queenBeeObject = GameObject.FindGameObjectsWithTag("QueenBee")[0];
        Animator queenBeeAnimator = queenBeeObject.GetComponent<Animator>();
        queenBeeAnimator.SetTrigger("Attack");
        
        PlayEarCharacterAnimation();
        
        
        
        
    }

     private void PlayEarCharacterAnimation()
    {
        Debug.Log("animation object" + animationObject);
        
        animationObject.GetComponent<SpriteRenderer>().enabled = true;
        GameObject character = GameObject.FindGameObjectsWithTag("Player")[0];
        Debug.Log(" character" + character);
        character.SetActive(false);

    }

}
