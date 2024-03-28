using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots; 
    public Item defaultItem;
    public GameObject defaultItemPrefab;

    public GameObject extendedInventoryPanel;
    public ExtendedInventoryManager extendedInventoryManager; 
    public Button backpackButton;

    public bool AddItem(Item item)
    {
        foreach (GameObject slot in inventorySlots)
        {
            Transform itemIconTransform = slot.transform.GetChild(0);
            if (!itemIconTransform.gameObject.activeInHierarchy)
            {
                itemIconTransform.gameObject.SetActive(true);
                Image itemImage = itemIconTransform.GetComponent<Image>();
                itemImage.sprite = item.itemSprite;
                // itemIconTransform.localScale = new Vector3(0.7f, 0.7f, 1);

                // Check for an existing ItemRepresentation component or add one
                ItemRepresentation itemRep = itemIconTransform.GetComponent<ItemRepresentation>();
                if (itemRep == null)
                {
                    itemRep = itemIconTransform.gameObject.AddComponent<ItemRepresentation>();
                }
                itemRep.item = item;

                return true; // Item successfully added
            }
        }
        return false; // Inventory is full
    }

    public bool AddItem(Item item, GameObject itemGameObject = null)
    {
        // First, try adding to the main inventory
        if (TryAddItemToSlots(inventorySlots, item, itemGameObject))
        {
            return true;
        }

        // If main is full, try the extended inventory if it's active
        if (backpackButton.gameObject.activeSelf)
        {
            return TryAddItemToSlots(extendedInventoryManager.extendedInventorySlots, item, itemGameObject);
        }

        // Couldn't add to either inventory
        return false;
    }


   private bool TryAddItemToSlots(GameObject[] slots, Item item, GameObject itemGameObject)
    {
        foreach (GameObject slot in slots)
        {
            ItemRepresentation itemRep = slot.GetComponentInChildren<ItemRepresentation>(true); // Include inactive children
            if (itemRep != null && !itemRep.gameObject.activeSelf)
            {
                // Reactivate and update an existing but inactive ItemRepresentation
                itemRep.item = item;
                itemRep.gameObject.SetActive(true);
                UpdateItemVisuals(itemRep.gameObject, item);
                return true; // Item successfully reactivated
            }
            else if (slot.transform.childCount == 0)
            {
                // Create a new item if there's no child
                CreateNewItemInSlot(slot, item, itemGameObject);
                return true;
            }
        }
        return false; // No suitable slot found
    }

    private void CreateNewItemInSlot(GameObject slot, Item item, GameObject itemGameObject)
    {
        GameObject newItem = itemGameObject ?? Instantiate(item.itemPrefab, slot.transform);
        newItem.transform.SetAsFirstSibling(); // Ensure it's the first child for consistency
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;
        newItem.transform.localScale = Vector3.one;

        SetupNewItem(newItem, item);
    }


    private void SetupNewItem(GameObject newItem, Item item)
    {
        Image itemImage = newItem.GetComponent<Image>();
        if (itemImage == null) itemImage = newItem.AddComponent<Image>();
        itemImage.sprite = item.itemSprite;

        ItemRepresentation itemRep = newItem.GetComponent<ItemRepresentation>();
        if (itemRep == null) itemRep = newItem.AddComponent<ItemRepresentation>();
        itemRep.item = item;

        // Setup for DraggableItem, if applicable
        DraggableItem draggableItem = newItem.GetComponent<DraggableItem>();
        if (draggableItem == null) draggableItem = newItem.AddComponent<DraggableItem>();
        // Set draggableItem's originalSlot or other properties as needed
    }

    private void UpdateItemVisuals(GameObject itemObject, Item item)
    {
        // This method updates the visuals of an existing item representation
        Image itemImage = itemObject.GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = item.itemSprite;
        }
        else
        {
            Debug.LogError("Item representation does not have an Image component.");
        }
    }



    public void AddDefaultItemToSlot(GameObject slot)
    {
        if (slot.transform.childCount == 0)
        {
            // Instantiate the default item prefab as a child of the slot
            GameObject newItem = Instantiate(defaultItemPrefab, slot.transform);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.identity;
            newItem.transform.localScale = Vector3.one;

            // Configure the Image component with the default item's sprite
            Image image = newItem.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = defaultItem.itemSprite;
            }
            else
            {
                Debug.LogError("Item prefab does not have an Image component.");
            }

            // Configure the ItemRepresentation component with the default item data
            ItemRepresentation itemRep = newItem.GetComponent<ItemRepresentation>();
            if (itemRep != null)
            {
                itemRep.item = defaultItem;
            }
            else
            {
                Debug.LogError("Item prefab does not have an ItemRepresentation component.");
            }

            // Configure the DraggableItem component (if your item is draggable)
            DraggableItem draggableItem = newItem.GetComponent<DraggableItem>();
            if (draggableItem != null)
            {
                draggableItem.originalSlot = slot; // Assuming you have this field defined in your DraggableItem script
            }
            else
            {
                Debug.LogError("Item prefab does not have a DraggableItem component.");
            }
        }
    }


    [YarnFunction("isItemInInventory")]
    public static bool isItemInInventory(string itemName) {
        
        // given a string with the name of an item, return true if at least one of that item is in the player's inventory
        return true;
    }

    [YarnCommand("addToInventory")]
    public void addToInventory(string itemName) {
        
        // given a string with the name of an item, add that item to the player's inventory
    }

    [YarnCommand("removeFromInventory")]
    public void removeFromInventory(string itemName) {
        if (isItemInInventory(itemName)) {
             // given a string with the name of an item, remove that item from the player's inventory
        } else {
            Debug.LogError("Unable to remove item " + itemName + " from player inventory.");
        }
       
    }
}
