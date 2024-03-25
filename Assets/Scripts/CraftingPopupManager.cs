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

    public void CraftItem()
    {
        foreach (var recipe in recipes)
        {
            if (RecipeMatches(recipe))
            {
                inventoryManager.AddItem(recipe.resultItem);
                ClearCraftingSlots();
                HideCraftingPopup();
                break;
            }
        }
    }

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

}
