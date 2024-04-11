
// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkatePole : MonoBehaviour
{
    private InteractIcon interactIcon;
    private bool playerIsNear = false;

    private Animator animator;

    [SerializeField] GameObject iceSkatePickUpItem;

    [SerializeField] GameObject iceSkateVisual;


    private void Awake()
    {
        interactIcon = GetComponentInChildren<InteractIcon>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;

    }

    private void SubmitPressed()
    {
        // Do nothing if the skate has already fallen
        if(!iceSkateVisual.activeInHierarchy) {
            return;
        }
        if (playerIsNear)
        {
            bool playerHasRakeRake = InventoryManager.isItemInInventory("RakeRake");
            if (playerHasRakeRake)
            {
                // knock down ice skate
                StartCoroutine(FallingAnimationCoroutine());


            }
            else
            {
                // unable to get the skate down - hit your hand on it
                // TODO: Run a hitting animation
                DialogueManager.instance.RunDialogueNode("Punch_Pole");

            }


        }

    }

    private IEnumerator FallingAnimationCoroutine()
    {
        animator.enabled = true;
        animator.Play("IceSkateFall", -1, 0f);
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        animator.enabled = false;
        iceSkatePickUpItem.SetActive(true);
        iceSkateVisual.SetActive(false);
        

    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        

        if (otherCollider.CompareTag("Player"))
        {
            interactIcon.SetState(active: true, locked: false);
            playerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
         
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            interactIcon.SetState(active: false, locked: false);
        }
    }
}
