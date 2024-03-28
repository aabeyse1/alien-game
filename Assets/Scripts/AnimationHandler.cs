using System.Collections;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
public static AnimationHandler Instance { get; private set; }

    public GameObject eatingAnimation;
    public GameObject character;
    public SpriteRenderer leftItemRenderer, rightItemRenderer, resultItemRenderer;

    public Animator animator;

    public CraftingPopupManager craftingManager;
    private CraftingRecipe currentRecipe;

    void Awake()
    {
        // Ensure that there's only one instance of the AnimationHandler
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // craftingManager = FindObjectOfType<CraftingPopupManager>(); // Ideally, you'd pass this reference instead of finding it.
        animator = eatingAnimation.GetComponent<Animator>(); // Cache the Animator component.
    }

    public void StartCraftingAnimation(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
        SetupAnimationSprites(recipe);
        StartCoroutine(CraftingAnimationCoroutine());
    }

    private IEnumerator CraftingAnimationCoroutine()
    {
        animator.Play("Eating", -1, 0f);
        
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        
        OnAnimationComplete();
    }


    private void OnAnimationComplete()
    {
        eatingAnimation.SetActive(false);
        character.SetActive(true);

        if (currentRecipe != null)
        {
            craftingManager.ExecuteCrafting(currentRecipe);
        }
        else
        {
            Debug.LogError("The recipe is null. Cannot execute crafting.");
        }
    }

    private void SetupAnimationSprites(CraftingRecipe recipe)
    {
        leftItemRenderer.sprite = recipe.requiredItems.Length > 0 ? recipe.requiredItems[0].itemSprite : null;
        rightItemRenderer.sprite = recipe.requiredItems.Length > 1 ? recipe.requiredItems[1].itemSprite : null;
        resultItemRenderer.sprite = recipe.resultItem.itemSprite;
    }
}
