using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IDropHandler, IBeginDragHandler
{
    public CraftingPopupManager craftingPopupManager;
    public int SlotIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag Detected on Crafting Slot");
        ItemRepresentation itemRep = GetComponentInChildren<ItemRepresentation>();
        if (itemRep && itemRep.item)
        {
            craftingPopupManager.ItemRemovedFromSlot(this, itemRep.item);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DraggableItem.itemBeingDragged != null)
        {
            DraggableItem.itemBeingDragged.transform.SetParent(transform);
            DraggableItem.itemBeingDragged.transform.position = transform.position;

            ItemRepresentation itemRep = DraggableItem.itemBeingDragged.GetComponent<ItemRepresentation>();
            if (itemRep != null)
            {
                Item droppedItem = itemRep.item;
                craftingPopupManager.ItemDroppedInSlot(this, droppedItem);
            }
            else
            {
                Debug.LogWarning("Dropped item does not have an ItemRepresentation component or is null.");
            }
        }
    }
}
