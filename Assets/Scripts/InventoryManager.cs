using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots; 
    public Item defaultItem;
    public GameObject defaultItemPrefab;

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
    foreach (GameObject slot in inventorySlots)
    {
        if (slot.transform.childCount == 0)
        {
            GameObject newItem;
            
            if (itemGameObject == null)
            {
                newItem = Instantiate(item.itemPrefab, slot.transform);
            }
            else
            {
                newItem = itemGameObject;
                newItem.transform.SetParent(slot.transform, false);
                newItem.transform.localPosition = Vector3.zero;
            }
            return true;
        }
    }
    return false;
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
}
