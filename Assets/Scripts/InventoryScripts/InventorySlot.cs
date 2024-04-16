using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingPopupManager craftingPopupManager;

    // public void OnDrop(PointerEventData eventData)
    // {
    //     DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
    //     if (draggableItem == null)
    //     {
    //         Debug.LogError("DraggableItem component not found on the dragged object.");
    //         return; // Exit if we can't find DraggableItem to prevent null reference
    //     }

    //     ItemRepresentation itemRep = draggableItem.GetComponent<ItemRepresentation>();
    //     if (itemRep == null || itemRep.item == null)
    //     {
    //         Debug.LogError("ItemRepresentation component is missing or item is null.");
    //         return; // Exit to prevent null reference
    //     }

    //     // Ensure that inventoryManager is assigned and not null
    //     if (inventoryManager == null)
    //     {
    //         Debug.LogError("InventoryManager reference is not set.");
    //         return; // Exit to prevent null reference
    //     }

    //     bool wasAdded = inventoryManager.AddItem(itemRep.item, draggableItem.gameObject);
    // }

}
