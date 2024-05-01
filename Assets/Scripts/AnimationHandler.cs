using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimationHandler : MonoBehaviour
{
public static AnimationHandler Instance { get; private set; }

    public GameObject eatingAnimation;
    public GameObject character;
    public SpriteRenderer leftItemRenderer, rightItemRenderer, resultItemRenderer;

    public Animator animator;

    public CraftingPopupManager craftingManager;
    private CraftingRecipe currentRecipe;
    public Camera mainCamera;
    private float originalFOV;
    private Vector3 originalCameraPosition;
    public Light2D surroundingLight;

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
        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
            originalCameraPosition = mainCamera.transform.position;
        }
    }

    public void StartCraftingAnimation(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
        SetupAnimationSprites(recipe);
        // ResetCameraToCharacter();
        StartCoroutine(CraftingAnimationCoroutine());
        StartCoroutine(ZoomInOnCharacter());
    }

    public void ResetCameraToCharacter()
    {
        if (mainCamera != null && character != null)
        {
            originalCameraPosition = mainCamera.transform.position;
            mainCamera.transform.position = character.transform.position + new Vector3(0, 0, -10);
            mainCamera.transform.position = character.transform.position + new Vector3(0, 0, -10);
        }
    }

    private IEnumerator CraftingAnimationCoroutine()
    {

        animator.Play("Eating", -1, 0f);
        
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        
        OnAnimationComplete();
    }

    private IEnumerator ZoomInOnCharacter()
    {

        if (mainCamera != null)
        {
            mainCamera.transform.position = character.transform.position + new Vector3(0, 0, -10);
            Debug.Log("Starting camera zoom.");
            float duration = 1.0f;
            float elapsedTime = 0;
            float targetOrthographicSize = 0.45f;
            float originalOrthographicSize = mainCamera.orthographicSize;
            Vector3 targetPosition = character.transform.position + new Vector3(0, 0, -10);
            float originalLightIntensity = surroundingLight.intensity;
            float targetLightIntensity = 0.05f;

            while (elapsedTime < duration)
            {
                mainCamera.orthographicSize = Mathf.Lerp(originalOrthographicSize, targetOrthographicSize, elapsedTime / duration);
                mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, targetPosition, elapsedTime / duration);
                surroundingLight.intensity = Mathf.Lerp(originalLightIntensity, targetLightIntensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mainCamera.orthographicSize = targetOrthographicSize;
            mainCamera.transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("Main camera is not assigned.");
        }
    }

    private IEnumerator ResetCameraZoom()
    {
        if (mainCamera != null)
        {
            float duration = 1.0f;
            float elapsedTime = 0;
            float targetOrthographicSize = 0.8f;
            float originalOrthographicSize = mainCamera.orthographicSize;
            Vector3 targetPosition = character.transform.position + new Vector3(0, 0, -10);
            float originalLightIntensity = surroundingLight.intensity;
            float targetLightIntensity = 0.6f;
            while (elapsedTime < duration)
            {
                float lerpFactor = elapsedTime / duration;
                mainCamera.orthographicSize = Mathf.Lerp(originalOrthographicSize, targetOrthographicSize, elapsedTime / duration);
                mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, targetPosition, elapsedTime / duration);
                surroundingLight.intensity = Mathf.Lerp(originalLightIntensity, targetLightIntensity, lerpFactor);                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mainCamera.orthographicSize = targetOrthographicSize;
            mainCamera.transform.position = targetPosition;
            surroundingLight.intensity = targetLightIntensity;
            // mainCamera.orthographicSize = 0.8f; // Reset to original orthographic size
            // mainCamera.transform.position = originalCameraPosition;
            Debug.Log("Camera reset to original settings.");
        }
    }


    private void OnAnimationComplete()
    {
        surroundingLight.intensity = 0.6f;
        StartCoroutine(ResetCameraZoom());
        ResetCameraToCharacter();

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
