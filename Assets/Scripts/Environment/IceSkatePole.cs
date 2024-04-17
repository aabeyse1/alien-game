
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

    [SerializeField] GameObject pokingAnimationPrefab;

    private GameObject character;
    private GameObject animationObject;

    private void Awake()
    {
        interactIcon = GetComponentInChildren<InteractIcon>();
        animator = GetComponent<Animator>();
        character = GameObject.FindGameObjectsWithTag("Player")[0];
        
        animator.Play("IceSkateSwinging", -1, 0f);
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
                PlayPokingCharacterAnimation();
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
    private void PlayPokingCharacterAnimation() {
        animationObject = Instantiate(pokingAnimationPrefab);
        
        // put the animation at an offset position to account for the difference in sprite sizes
        Vector3 characterPos = character.transform.position;
        animationObject.transform.position = new Vector3(characterPos.x, characterPos.y + 0.1f, characterPos.z);

        // face left or right
        if (character.transform.position.x > transform.position.x)  {
            animationObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }
    private IEnumerator FallingAnimationCoroutine()
    {
        character.SetActive(false);
        animator.SetBool("Poked", true);
        
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Exit"));
        
        animator.enabled = false;
        iceSkatePickUpItem.SetActive(true);
        iceSkateVisual.SetActive(false);
        Destroy(animationObject);
        character.SetActive(true);
        

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
