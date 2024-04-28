using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    private Vector3 startPosition;
    private Transform startParent;
    private CanvasGroup canvasGroup;
    public CraftingPopupManager craftingManager;

    public GameObject originalSlot;
    private Vector3 originalScale;
    public InventoryManager inventoryManager;

    public Item defaultItem;
    public int? originatingSlotIndex = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
        craftingManager = FindObjectOfType<CraftingPopupManager>(true);
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager instance not found in the scene.");
        }
        originalSlot = transform.parent.gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.localPosition; 
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        CraftingSlot originatingSlot = startParent.GetComponent<CraftingSlot>();
        if (originatingSlot != null)
        {
            originatingSlotIndex = originatingSlot.SlotIndex;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)startParent, eventData.position, eventData.pressEventCamera, out newPos);
        transform.localPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        canvasGroup.blocksRaycasts = true;

        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found.");
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(eventData.pointerId))
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            RaycastResult result = results.Find(r => r.gameObject.CompareTag("CraftingSlot") || r.gameObject.CompareTag("InventorySlot"));

            if (result.gameObject != null)
            {
                GameObject targetObject = result.gameObject.transform.childCount > 0 ? result.gameObject.transform.GetChild(0).gameObject : null;
                Transform originalParent = startParent; 

                bool originalParentIsCraftingSlot = originalParent.CompareTag("CraftingSlot");
                int? originalSlotIndex = GetSlotIndex(originalParent);

                if (result.gameObject.CompareTag("CraftingSlot"))
                {
                    Debug.Log("Dropped on Crafting Slot");
                    
                    if (originalParentIsCraftingSlot)
                    {
                        if (originalSlotIndex.HasValue)
                        {
                            craftingManager.ClearItemFromSlot(originalSlotIndex.Value);
                        }
                    }

                    SetItemToSlot(GetComponent<ItemRepresentation>(), result.gameObject.transform, originalSlotIndex);
                }
                else if (result.gameObject.CompareTag("InventorySlot"))
                {
                    if (startParent == result.gameObject.transform)
                    {
                        ResetItemPosition();
                    }
                    else
                    {
                        if (originatingSlotIndex.HasValue)
                        {
                            craftingManager.ClearItemFromSlot(originatingSlotIndex.Value);
                            originatingSlotIndex = null; // Reset the index after clearing
                        }
                        ResetItemPositionToSlot(result.gameObject.transform);
                            
                    }
                }
                else
                {
                    ResetItemPosition();
                }
            }
            else
            {
                ResetItemPosition();
            }
        }
        else if (!inventoryManager.extendedInventoryPanel.activeSelf)
        {
            Vector3 dropPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            dropPosition.z = 1;

            ItemRepresentation itemRep = GetComponent<ItemRepresentation>();
            if (itemRep != null)
            {
                // inventoryManager.PlaceItemInWorld(itemRep.item.itemName, dropPosition);
                ResetItemPosition();
            }
            else
            {
                Debug.LogError("ItemRepresentation not found on the dragged item.");
            }
        }
        else
        {
            ResetItemPosition();
        }
    }

    private int? GetSlotIndex(Transform slotTransform)
    {
        CraftingSlot slot = slotTransform.GetComponent<CraftingSlot>();
        return slot ? slot.SlotIndex : null;
    }


private void SetItemToSlot(ItemRepresentation itemRep, Transform slotTransform, int? originatingSlotIndex)
{
    // Retrieve the existing item representation in the target slot, if any
    ItemRepresentation targetSlotItemRep = slotTransform.GetComponentInChildren<ItemRepresentation>(true);

    if (targetSlotItemRep != null)
    {
        // If the target slot already has an item, we need to swap the GameObjects
        GameObject tempGameObject = targetSlotItemRep.gameObject; // Store the existing GameObject temporarily
        Transform originalParent = itemRep.transform.parent; // Store the original parent

        // Swap the GameObjects
        tempGameObject.transform.SetParent(originalParent);
        tempGameObject.transform.localPosition = Vector3.zero;
        tempGameObject.transform.localRotation = Quaternion.identity;
        tempGameObject.transform.localScale = Vector3.one;

        itemRep.gameObject.transform.SetParent(slotTransform);
        itemRep.gameObject.transform.localPosition = Vector3.zero;
        itemRep.gameObject.transform.localRotation = Quaternion.identity;
        itemRep.gameObject.transform.localScale = Vector3.one;
    }
    else
    {
        // If no item is in the slot, directly move the GameObject to the new slot
        itemRep.gameObject.transform.SetParent(slotTransform);
        itemRep.gameObject.transform.localPosition = Vector3.zero;
        itemRep.gameObject.transform.localRotation = Quaternion.identity;
        itemRep.gameObject.transform.localScale = Vector3.one;
    }

    // Update crafting slots and craftability if necessary
    if (originatingSlotIndex.HasValue)
    {
        craftingManager.ClearItemFromSlot(originatingSlotIndex.Value);
    }
    craftingManager.UpdateCraftability(); // Update craftability after the item is set
}




    private void ResetItemPositionToSlot(Transform slotTransform)
    {
        GameObject draggedObject = gameObject;
        GameObject targetObject = slotTransform.childCount > 0 ? slotTransform.GetChild(0).gameObject : null;

        Transform originalParent = startParent; // Remember original parent for swapping back if needed

        // Check if the original parent had the 'CraftingSlot' tag
        bool originalParentIsCraftingSlot = originalParent.CompareTag("CraftingSlot");
        bool originalParentIsInvSlot = originalParent.CompareTag("InventorySlot");

        if (targetObject != null)
        {
            // Swap the children between the slots
            targetObject.transform.SetParent(originalParent, false);
            targetObject.transform.localPosition = Vector3.zero;
            targetObject.transform.localScale = originalScale;

            draggedObject.transform.SetParent(slotTransform, false);
            draggedObject.transform.localPosition = Vector3.zero;
            draggedObject.transform.localScale = originalScale;

            // If the original slot was a crafting slot, destroy the child now placed there
            if (originalParentIsCraftingSlot)
            {
                craftingManager.UpdateCraftability();
                Destroy(targetObject); // Destroy the target object which is now in the original crafting slot
            }
            if (originalParentIsInvSlot)
            {
                Debug.Log("replacing item here");
                Destroy(targetObject);
                Debug.Log(originalParent.childCount);
                craftingManager.UpdateCraftability();
                StartCoroutine(inventoryManager.AddDefaultItemAfterFrame(originalParent.gameObject));
            }
        }
        else
        {
            // If the target slot is empty, simply move the dragged object to the target slot
            draggedObject.transform.SetParent(slotTransform, false);
            draggedObject.transform.localPosition = Vector3.zero;
            draggedObject.transform.localScale = originalScale;
        }

        // Add a default item to the original slot if it's not a crafting slot
        if (!originalParentIsCraftingSlot && originalParent.childCount == 0)
        {
            inventoryManager.AddDefaultItemToSlot(originalParent.gameObject);
        }
    }


    private void UpdateItemVisuals(GameObject itemObject, Item item)
    {
        Image itemImage = itemObject.GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = item.itemSprite;
        }
        else
        {
            Debug.LogError("Item representation does not have an Image component.", itemObject);
        }
    }



    private void SetItemParentAndPosition(Transform newParent)
    {
        transform.SetParent(newParent, false);
        transform.localPosition = Vector3.zero;
        transform.localScale = originalScale; // Reset any scale changes
    }

    private void ResetItemPosition()
    {
        transform.localPosition = startPosition;
        transform.SetParent(startParent, false);
        transform.localScale = originalScale; // Reset scale to original
    }

    private void PlaceItemOnMap(PointerEventData eventData)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPosition.z = 1;

        Debug.Log($"Placing item at {worldPosition}");
    }




    public void ResetDraggable()
    {
        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(true);
        transform.SetParent(startParent);
        transform.localPosition = startPosition;
        transform.localScale = originalScale;
    }
}
