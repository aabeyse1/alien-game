using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots; 
    public Item defaultItem;
    public GameObject defaultItemPrefab;

    public GameObject extendedInventoryPanel;
    public ExtendedInventoryManager extendedInventoryManager; 
    public Button backpackButton;

    public ItemDatabase itemDatabase;

    public static List<string> itemsInInventory = new List<string>();

    private Dictionary<string, GameObject> itemMap; // links item name to its prefab

    // public GameObject beltPrefab;
    // public GameObject hammerPrefab;
    // public GameObject sockPrefab;
    // public GameObject stickPrefab;
    // public GameObject quiltPrefab;
    // public GameObject axePrefab;
    // public GameObject logPrefab;
    // public GameObject skatePrefab;
    // public GameObject rakePrefab;
    // public GameObject pipePrefab;
    // public GameObject rakeRakePrefab;

    private void Awake() {
        itemMap = CreateItemMap();
    }

    private Dictionary<string, GameObject> CreateItemMap() {
         // loads all Item scriptable objects under the Assets/Resources/Quests folder
        GameObject[] allItems = Resources.LoadAll<GameObject>("ItemPrefabs");

         // create the item map
        Dictionary<string, GameObject> idToItemMap = new Dictionary<string, GameObject>();
        foreach (GameObject itemSO in allItems) {
            if (idToItemMap.ContainsKey(itemSO.name))
            {
                Debug.LogWarning("Duplicate ID found when creating item map: " + itemSO.name);
            }
            idToItemMap.Add(itemSO.name, itemSO);
        }
        return idToItemMap;
    }

     // catch errors if we try to access an item that doesn't exist
    public GameObject GetItemByName(string name)
    {
        GameObject item = itemMap[name];
        if (item == null)
        {
           Debug.LogError("GameObject not found for item: " + name);
        }
        return item;
    }

    


    public void PlaceItemInWorld(string itemName, Vector3 worldPosition)
    {
        GameObject itemGameObject = GetItemByName(itemName);
        if (itemGameObject != null)
        {
            Debug.Log("set game object active = " + itemGameObject);
            
            GameObject drop = Instantiate(itemGameObject);
            drop.transform.position = worldPosition;


            RemoveFromInventory(itemName);
        }
        else
        {
            Debug.LogError($"GameObject for item '{itemName}' not found.");
        }
    }

    public void RemoveFromInventory(string itemName)
    {
        if (itemsInInventory.Contains(itemName))
        {
            itemsInInventory.Remove(itemName);

            IEnumerable<GameObject> allSlots = inventorySlots.Concat(extendedInventoryManager.extendedInventorySlots);

            foreach (GameObject slot in allSlots)
            {
                ItemRepresentation itemRep = slot.GetComponentInChildren<ItemRepresentation>(true); // Include inactive children
                if (itemRep != null && itemRep.item.itemName == itemName)
                {
                    Destroy(itemRep.gameObject); // Destroy the item representation GameObject
                    StartCoroutine(AddDefaultItemAfterFrame(slot));
                    break; // Exit after handling the item
                }
            }
        }
        else
        {
            Debug.LogError("Item not in inventory: " + itemName);
        }
    }

    private IEnumerator AddDefaultItemAfterFrame(GameObject slot)
    {
        yield return null; 
        AddDefaultItemToSlot(slot);
    }




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

                itemsInInventory.Add(item.itemName);
                return true;
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
                itemsInInventory.Add(item.itemName);
                return true; // Item successfully reactivated
            }
            else if (slot.transform.childCount == 0)
            {
                // Create a new item if there's no child
                CreateNewItemInSlot(slot, item, itemGameObject);
                itemsInInventory.Add(item.itemName);
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

        DraggableItem draggableItem = newItem.GetComponent<DraggableItem>();
        if (draggableItem == null) draggableItem = newItem.AddComponent<DraggableItem>();
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
            Debug.Log("amde it inside childcount if");
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
                // draggableItem.originalSlot = slot;
            }
            else
            {
                Debug.LogError("Item prefab does not have a DraggableItem component.");
            }
        }
    }


    [YarnFunction("isItemInInventory")]
    public static bool isItemInInventory(string itemName)
    {
        return itemsInInventory.Contains(itemName);
    }

   


    [YarnCommand("addToInventory")]
    public void addToInventory(string itemName)
    {
        Item item = itemDatabase.GetItemByName(itemName);
        if (item != null)
        {
            bool added = AddItem(item);
            if (!added)
            {
                Debug.LogError("Failed to add item to inventory: " + itemName);
            }
        }
        else
        {
            Debug.LogError("Item not found in database: " + itemName);
        }
    }


    [YarnCommand("removeFromInventory")]
    public void RemoveFromInventoryCommand(string itemName)
    {
        // Check if the item is in the inventory
        if (itemsInInventory.Contains(itemName))
        {
            // Remove the item name from the tracking list
            itemsInInventory.Remove(itemName);

            // Find the slot that contains this item and remove it
            foreach (GameObject slot in inventorySlots)
            {
                ItemRepresentation itemRep = slot.GetComponentInChildren<ItemRepresentation>();
                if (itemRep != null && itemRep.item.itemName == itemName)
                {
                    // Deactivate the current item representation
                    itemRep.gameObject.SetActive(false);

                    // Add a default item to this slot
                    AddDefaultItemToSlot(slot);
                    return; // Exit after dealing with the first occurrence
                }
            }

            // If the item wasn't found in the main inventory, check the extended inventory
            foreach (GameObject slot in extendedInventoryManager.extendedInventorySlots)
            {
                ItemRepresentation itemRep = slot.GetComponentInChildren<ItemRepresentation>();
                if (itemRep != null && itemRep.item.itemName == itemName)
                {
                    // Deactivate the current item representation
                    itemRep.gameObject.SetActive(false);

                    // Add a default item to this slot
                    AddDefaultItemToSlot(slot);
                    return; // Exit after dealing with the first occurrence
                }
            }
        }
        else
        {
            Debug.LogError("Item not in inventory: " + itemName);
        }
    }

}
