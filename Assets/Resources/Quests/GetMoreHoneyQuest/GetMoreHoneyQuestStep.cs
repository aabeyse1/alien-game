// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class GetMoreHoneyQuestStep : QuestStep // inherit from QuestStep
{

public static GetMoreHoneyQuestStep instance { get; private set; }

private GameObject tutorialObject;

 private GameObject queenBeeObject; 


private GameObject animationObject;


   
    private void Awake() {
        instance = this;
        animationObject = GameObject.FindGameObjectsWithTag("EarSpinAnimationPrefab")[0];
        Debug.Log("AWAKE instance = " + instance);
        queenBeeObject = GameObject.FindGameObjectsWithTag("QueenBee")[0];
        tutorialObject = GameObject.Find("AttackTutorial");
    }

   
     private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onSpacePressed += SpacePressed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onSpacePressed -= SpacePressed;

    }


    private void SpacePressed() {
        if (tutorialObject.activeInHierarchy) {
            // Attack
            Animator queenBeeAnimator = queenBeeObject.GetComponent<Animator>();
            queenBeeAnimator.SetTrigger("Attack");
            tutorialObject.GetComponent<SpriteRenderer>().enabled = false;
            PlayEarCharacterAnimation();
            
        }
    }


    protected override void SetQuestStepState(string state)
    {
        
    }

    [YarnCommand("ShowAttackOption")]
    public void ShowAttackButton() {
        tutorialObject.GetComponent<SpriteRenderer>().enabled = true;
        // TODO: freeze player movement ?
    }

    

     private void PlayEarCharacterAnimation()
    {
        Debug.Log("animation object" + animationObject);
        animationObject.GetComponent<Animator>().enabled = true;
        animationObject.GetComponent<SpriteRenderer>().enabled = true;
        GameObject character = GameObject.FindGameObjectsWithTag("Player")[0];
        Debug.Log(" character" + character);
        character.SetActive(false);

    }

}
