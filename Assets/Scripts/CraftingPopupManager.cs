using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CraftingPopupManager : MonoBehaviour
{
    public GameObject craftingPopup;
    public Image[] craftingSlots; 
    public Button craftButton;

    public Button mainButton;
    public Button hideButton;

    public Button extendedInventoryButton;


    private Item[] itemsInSlots = new Item[2]; // To store items in slots
    public InventoryManager inventoryManager; // Reference to manage inventory
    public CraftingRecipe[] recipes; 

    void Start()
    {
        craftButton.interactable = false;
    }

    public void ShowCraftingPopup()
    {
        craftingPopup.SetActive(true);
        mainButton.gameObject.SetActive(false);
        hideButton.gameObject.SetActive(true);
        Time.timeScale = 0; // Freeze game, excluding inventory interactions
    }

    public void HideCraftingPopup()
    {
        foreach (Image craftingSlot in craftingSlots)
        {
            foreach (Transform child in craftingSlot.transform)
            {
                DraggableItem draggableItem = child.GetComponent<DraggableItem>();
                if (draggableItem != null && draggableItem.originalSlot != null)
                {
                    GameObject originalSlot = draggableItem.originalSlot;
                    child.SetParent(originalSlot.transform, false);
                    child.localPosition = Vector3.zero;
                    child.gameObject.SetActive(true);
                }
            }
        }

        craftingPopup.SetActive(false);
        mainButton.gameObject.SetActive(true);
        hideButton.gameObject.SetActive(false);
        Time.timeScale = 1;
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
        int slotIndex = System.Array.IndexOf(craftingSlots, slot.GetComponent<Image>());
        if (slotIndex != -1)
        {
            itemsInSlots[slotIndex] = item; // Store the dropped item
            UpdateCraftability(); // Re-evaluate crafting availability
        }
    }

 public void UpdateCraftability()
{
    craftButton.interactable = recipes.Any(recipe => RecipeMatches(recipe));
}


    private bool RecipeMatches(CraftingRecipe recipe)
    {
        Debug.Log($"Checking recipe: {recipe.requiredItems[0].itemName} + {recipe.requiredItems[1].itemName}");
        return (itemsInSlots[0] == recipe.requiredItems[0] && itemsInSlots[1] == recipe.requiredItems[1]) ||
               (itemsInSlots[0] == recipe.requiredItems[1] && itemsInSlots[1] == recipe.requiredItems[0]);
    }

    public GameObject itemPrefab; // Assign the default item prefab in the inspector

    public void ItemRemovedFromSlot(CraftingSlot slot, Item item)
    {
        // Find the slot index and clear the corresponding item
        int slotIndex = System.Array.IndexOf(craftingSlots, slot.GetComponent<Image>());
        Debug.Log("Item is removed");
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
            // Special case for crafting a backpack
            if (recipe.resultItem.itemName == "Backpack") // Replace with your actual item name check
            {
                extendedInventoryButton.gameObject.SetActive(true);
                // No need to add to inventory since it's an extension
            }
            else
            {
                // For all other items, instantiate and add to the inventory
                GameObject resultItemPrefab = Instantiate(recipe.resultItem.itemPrefab);
                resultItemPrefab.transform.localScale = new Vector3(1f, 1f, 1f); // Adjust scale as needed

                // Attempt to add the item to inventory
                bool wasAdded = inventoryManager.AddItem(recipe.resultItem, resultItemPrefab);
                if (!wasAdded)
                {
                    Debug.LogError("No space in inventory for crafted item.");
                    Destroy(resultItemPrefab); // Cleanup
                }
            }

            // Clear used items from crafting slots
            ClearUsedItemsInCraftingSlots();

            // Add default items to any empty slots left in the inventory
            AddDefaultItemsToEmptySlots();

            return; // Exit the loop as we've handled the crafting
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
    // Check each inventory slot and add default item if empty
    foreach (GameObject slot in inventoryManager.inventorySlots)
    {
        if (slot.transform.childCount == 0)
        {
            inventoryManager.AddDefaultItemToSlot(slot);
        }
    }
}

}
