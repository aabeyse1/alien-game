using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class CraftingPopupManager : MonoBehaviour
{
    public GameObject craftingPopup;
    public Image[] craftingSlots; 
    public Button craftButton;
    public Button mainButton;
    public Button hideButton;
    public Button extendedInventoryButton;
    public Item[] itemsInSlots = new Item[2]; // To store items in slots
    public InventoryManager inventoryManager; // Reference to manage inventory
    public CraftingRecipe[] recipes; 
    private CraftingRecipe currentRecipe;
    public AnimationHandler animationHandler;

    public GameObject EatingAnimation;
    public GameObject Character;
    public Camera mainCamera;

    private float originalFOV; // To store the original field of view of the camera
    private Vector3 originalCameraPosition; 

    [SerializeField] public GameObject draggingTutorialObject;
    public ExtendedInventoryManager extendedInventoryManager; 
    public Button backpackButton;
    

    // private bool isAnimationPlaying = false;

    void Start()
    {
        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
            originalCameraPosition = mainCamera.transform.position;
        }
        craftButton.interactable = false;
    }

    void Awake() 
    {
        recipes = Resources.LoadAll<CraftingRecipe>("CraftingRecipes");
    }

    public void ExecuteCrafting(CraftingRecipe recipe)
    {
        AddDefaultItemsToEmptySlots();
        if (recipe.resultItem.itemName == "Backpack")
        {
            // Special logic for backpack
            extendedInventoryButton.gameObject.SetActive(true);
        }
        else
        {
            // Proceed with crafting for non-special items
            GameObject resultItemPrefab = Instantiate(recipe.resultItem.itemPrefab);
            resultItemPrefab.SetActive(false);
            // resultItemPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            // Debug.Log("this is the item");
            // Debug.Log(resultItemPrefab);
            bool wasAdded = inventoryManager.AddItem(recipe.resultItem, resultItemPrefab);
            if (!wasAdded)
            {
                Debug.LogError("No space in inventory for crafted item.");
                Destroy(resultItemPrefab);
            }
        }

        GameEventsManager.instance.pickUpEvents.ItemCrafted(recipe.resultItem.itemName);

        ClearUsedItemsInCraftingSlots();

        IEnumerable<GameObject> allSlots = backpackButton.gameObject.activeSelf ?
        inventoryManager.inventorySlots.Concat(extendedInventoryManager.extendedInventorySlots) : inventoryManager.inventorySlots;

        foreach (GameObject slot in allSlots)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot != null)
            {
                inventorySlot.SetEquipped(false);
                inventorySlot.outline.enabled = false;
            }
        }

    // Reset the current equipped slot to null
        inventoryManager.characterEquipManager.currentEquippedSlot = null;
    }

    public void ShowCraftingPopup()
    {
        craftingPopup.SetActive(true);
        mainButton.gameObject.SetActive(false);
        hideButton.gameObject.SetActive(true);
        Time.timeScale = 0; // Freeze game, excluding inventory interactions
        if (EatingAnimation != null && Character != null) 
        {
            EatingAnimation.transform.position = Character.transform.position;
        }
        if (!TutorialManager.instance.hasDraggedItemForCrafting) {
                // if haven't poked yet, show the spacebar icon telling you how to use tools
                draggingTutorialObject.SetActive(true);
        }
    }

    public void HideCraftingPopup()
    {
        if (draggingTutorialObject) {
             draggingTutorialObject.SetActive(false);
        }
       
        foreach (Image craftingSlot in craftingSlots)
        {
            ItemRepresentation itemRep = craftingSlot.GetComponentInChildren<ItemRepresentation>(includeInactive: true);
            if (itemRep != null && itemRep.gameObject.activeSelf)
            {
                DraggableItem draggableItem = itemRep.GetComponent<DraggableItem>();
                if (draggableItem != null && draggableItem.originalSlot != null)
                {
                    // Reactivate the original slot's item representation and update it
                    ItemRepresentation originalSlotItemRep = draggableItem.originalSlot.GetComponentInChildren<ItemRepresentation>(true);
                    if (originalSlotItemRep != null)
                    {
                        originalSlotItemRep.item = itemRep.item; // Update with the new item
                        originalSlotItemRep.gameObject.SetActive(true);
                        Image itemImage = originalSlotItemRep.GetComponent<Image>();
                        if (itemImage != null) {
                            itemImage.enabled = true;
                            itemImage.sprite = itemRep.item.itemSprite;
                            Canvas.ForceUpdateCanvases(); // Force update all canvases
                        }

                    }

                    // Remove the item from the crafting slot
                    Destroy(itemRep.gameObject);
                }
            }
        }
        for (int i = 0; i < itemsInSlots.Length; i++)
        {
            itemsInSlots[i] = null;
        }
        craftingPopup.SetActive(false);
        mainButton.gameObject.SetActive(true);
        hideButton.gameObject.SetActive(false);
        UpdateCraftability();
        Time.timeScale = 1;
    }

    public void HideCraftingPopupCrafting()
    {
        // Clear items in crafting slots without moving them back
        foreach (Image craftingSlot in craftingSlots)
        {
            foreach (Transform child in craftingSlot.transform)
            {
                // Directly destroy the child items in the crafting slots
                Destroy(child.gameObject);
            }
        }

        // Update the UI components and resume the game
        craftingPopup.SetActive(false);
        mainButton.gameObject.SetActive(true);
        hideButton.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    public GameObject FindInventorySlotForItem(Item item)
    {
        foreach (GameObject slot in inventoryManager.inventorySlots)
        {
            ItemRepresentation itemRep = slot.GetComponentInChildren<ItemRepresentation>(true); // Include inactive children
            if (itemRep != null && itemRep.item == item)
            {
                // Found the slot containing the item.
                return slot;
            }
        }

        // If we're here, no slot was found with the item.
        return null;
    }



    public void ClearItemFromSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < itemsInSlots.Length)
        {
            itemsInSlots[slotIndex] = null;
            UpdateCraftability(); // Update the craft button's state
        }
    }

    private void ClearCraftingSlots()
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].GetComponentInChildren<ItemRepresentation>(includeInactive: true) is ItemRepresentation itemRep)
            {
                itemRep.gameObject.SetActive(false);
            }
            itemsInSlots[i] = null; // Clear stored item
        }
        UpdateCraftability(); // Update the craft button state after clearing slots
    }

    // Called by CraftingSlot when an item is dropped
    public void ItemDroppedInSlot(CraftingSlot slot, Item item)
    {   
        AddDefaultItemsToEmptySlots();
        int slotIndex = System.Array.IndexOf(craftingSlots, slot.GetComponent<Image>());
        if (slotIndex != -1)
        {
            itemsInSlots[slotIndex] = item;
            UpdateCraftability();
            
        }
    }

    public void DroppedItemUpdateTutorial() {
         // Tutorial won't show up anymore
            if (!TutorialManager.instance.hasDraggedItemForCrafting) {
                TutorialManager.instance.hasDraggedItemForCrafting = true;
                if (draggingTutorialObject) {
                    draggingTutorialObject.SetActive(false);
                }
            }
    }

    public void UpdateCraftability()
    {
        // Debug.Log(itemsInSlots[0]);
        // Debug.Log(itemsInSlots[1]);
        // Debug.Log("recipes: " + recipes.Length);
        craftButton.interactable = recipes.Any(recipe => RecipeMatches(recipe));
    }


    private bool RecipeMatches(CraftingRecipe recipe)
    {
        return (itemsInSlots[0] == recipe.requiredItems[0] && itemsInSlots[1] == recipe.requiredItems[1]) ||
               (itemsInSlots[0] == recipe.requiredItems[1] && itemsInSlots[1] == recipe.requiredItems[0]);
    }

    public GameObject itemPrefab; // Assign the default item prefab in the inspector

    public void ItemRemovedFromSlot(CraftingSlot slot, Item item)
    {
        // Find the slot index and clear the corresponding item
        int slotIndex = System.Array.IndexOf(craftingSlots, slot.GetComponent<Image>());
        // Debug.Log("Item is removed");
        if (slotIndex != -1)
        {
            itemsInSlots[slotIndex] = null; // Clear the removed item
            UpdateCraftability(); // Re-evaluate crafting availability
        }
    }

    public void OnCraftButtonClick()
    {
        foreach (var recipe in recipes)
        {
            if (RecipeMatches(recipe))
            {
                currentRecipe = recipe; // Store the current recipe
                
                // Activate the EatingAnimation GameObject and hide the character
                // It's crucial to ensure these lines are executed before starting the coroutine
                EatingAnimation.transform.position = Character.transform.position;
                animationHandler.ResetCameraToCharacter();
                
                Character.SetActive(false);
                EatingAnimation.SetActive(true);
                animationHandler.StartCraftingAnimation(recipe);

                HideCraftingPopupCrafting(); // Hide crafting popup if necessary
                return; // Exit the loop as crafting is handled
            }
        }
        Debug.LogWarning("No matching recipe found.");
    }

    private void ClearUsedItemsInCraftingSlots()
    {
        // Clear the items used in crafting
        foreach (Image slot in craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject); // Destroy the input items' GameObjects
            }
        }

        // Reset stored items in slots after crafting
        ClearCraftingSlots();
    }

    private void AddDefaultItemsToEmptySlots()
    {
        IEnumerable<GameObject> allSlots = backpackButton.gameObject.activeSelf ? 
        inventoryManager.inventorySlots.Concat(extendedInventoryManager.extendedInventorySlots) : inventoryManager.inventorySlots;

        // Check each inventory slot and add default item if empty
        foreach (GameObject slot in allSlots)
        {
            if (slot.transform.childCount == 0)
            {
                inventoryManager.AddDefaultItemToSlot(slot);
            }
        }
    }
}
