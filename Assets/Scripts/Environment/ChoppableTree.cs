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

    [SerializeField] GameObject choppingAnimationPrefab;

    private GameObject character;
    private GameObject animationObject;
    private CharacterEquipManager characterEquipManager;

    [SerializeField] GameObject spacebarTutorialObject;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        interactIcon = GetComponentInChildren<InteractIcon>();
        character = GameObject.FindGameObjectsWithTag("Player")[0];
        Transform visualsGameObject = gameObject.transform.Find("Visuals");

        characterEquipManager = character.GetComponentInChildren<CharacterEquipManager>();

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
            // Tutorial won't show up anymore
            if (!TutorialManager.instance.hasChoppedTree)
            {
                TutorialManager.instance.hasChoppedTree = true;
                if (spacebarTutorialObject)
                {
                    spacebarTutorialObject.SetActive(false);
                }
            }

            bool playerHasAxe = characterEquipManager.GetEquippedItemName() == "Axe";
            if (playerHasAxe)
            {
                // chop down tree
                PlayChoppingCharacterAnimation();


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

    private void PlayChoppingCharacterAnimation()
    {
        animationObject = Instantiate(choppingAnimationPrefab);

        animationObject.transform.position = character.transform.position;

        if (character.transform.position.x > transform.position.x)
        {
            animationObject.GetComponent<SpriteRenderer>().flipX = true;
        }

    }



    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {


            Debug.Log(activeVisual.GetComponent<SpriteRenderer>());
            activeVisual.GetComponent<SpriteRenderer>().color = highlightColor;
            playerIsNear = true;

            if (!TutorialManager.instance.hasChoppedTree)
            {
                // if haven't chopped a tree yet, show the spacebar icon telling you how to use tools
                spacebarTutorialObject.SetActive(true);
            }


        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            activeVisual.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            if (spacebarTutorialObject)
            {
                spacebarTutorialObject.SetActive(false);
            }

        }
    }

    private IEnumerator FallDownAnimationCoroutine()
    {
        character.SetActive(false);
        animator.enabled = true;
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        animator.enabled = false;


        GameObject log = Instantiate(logPrefab);
        log.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
        Destroy(animationObject);
        character.SetActive(true);
        Destroy(this.gameObject);

    }

}
