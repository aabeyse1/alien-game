using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots; 

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
                itemIconTransform.localScale = new Vector3(0.7f, 0.7f, 1);

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

public bool AddItem(Item item, GameObject itemGameObject)
{
    foreach (GameObject slot in inventorySlots)
    {
        if (slot.transform.childCount == 0)
        {
            itemGameObject.transform.SetParent(slot.transform, false);
            itemGameObject.transform.localPosition = Vector3.zero;

            DraggableItem draggableItem = itemGameObject.GetComponent<DraggableItem>();
            if (draggableItem == null)
            {
                draggableItem = itemGameObject.AddComponent<DraggableItem>();
            }
            draggableItem.originalSlot = slot; // Set original slot

            ItemRepresentation itemRep = itemGameObject.GetComponent<ItemRepresentation>();
            if (itemRep == null)
            {
                itemRep = itemGameObject.AddComponent<ItemRepresentation>();
            }
            itemRep.item = item;

            return true;
        }
    }
    return false;
}


}
