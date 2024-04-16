// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    private InteractIcon interactIcon;
    private bool playerIsNear = false;

    [SerializeField] GameObject logPrefab;

    
    private GameObject activeVisual;
    [SerializeField] Color highlightColor;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        interactIcon = GetComponentInChildren<InteractIcon>();
        Transform visualsGameObject = gameObject.transform.Find("Visuals");

        // finding first active child code is from: https://discussions.unity.com/t/get-first-active-child-gameobject/181486
        for (int i = 0; i < visualsGameObject.childCount; i++)
        {
            if (visualsGameObject.GetChild(i).gameObject.activeSelf == true)
            {
                activeVisual = visualsGameObject.GetChild(i).gameObject;
                
            }
        }

    }
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onSpacePressed += SpacePressed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onSpacePressed -= SpacePressed;

    }

    private void SpacePressed()
    {

        if (playerIsNear)
        {
            bool playerHasAxe = InventoryManager.isItemInInventory("Axe");
            if (playerHasAxe)
            {
                // chop down tree
                StartCoroutine(FallDownAnimationCoroutine());
               
            }
            else
            {
                // unable to chop down tree - hit your hand on it
                // TODO: Run punching tree animation
                DialogueManager.instance.RunDialogueNode("Punch_Tree");

            }


        }

    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
           
            
            Debug.Log(activeVisual.GetComponent<SpriteRenderer>());
             activeVisual.GetComponent<SpriteRenderer>().color = highlightColor;
            playerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            activeVisual.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

     private IEnumerator FallDownAnimationCoroutine()
    {
        animator.enabled = true;
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        animator.enabled = false;
       

        GameObject log = Instantiate(logPrefab);
        log.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);

        Destroy(this.gameObject);
    }

}
