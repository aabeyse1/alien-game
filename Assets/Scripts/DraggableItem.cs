using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    private Vector3 startPosition;
    private Transform startParent;
    private CanvasGroup canvasGroup;

    public GameObject originalSlot;
    private Vector3 originalScale;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.localPosition; 
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
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
                if (result.gameObject.CompareTag("CraftingSlot"))
                {
                    Debug.Log("Dropped on Crafting Slot");
                }
                else if (result.gameObject.CompareTag("InventorySlot"))
                {
                    ResetItemPositionToSlot(result.gameObject.transform);
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
                inventoryManager.PlaceItemInWorld(itemRep.item.itemName, dropPosition);
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

    private void ResetItemPositionToSlot(Transform slotTransform)
    {
        transform.SetParent(slotTransform, false);
        transform.localPosition = Vector3.zero;
        transform.localScale = originalScale; // Reset any scale changes
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
