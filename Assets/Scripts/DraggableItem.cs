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

    // Use a Raycast to determine if we're over an inventory slot
    if (EventSystem.current.IsPointerOverGameObject(eventData.pointerId))
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        RaycastResult result = results.Find(r => r.gameObject.CompareTag("InventorySlot"));

        if (result.gameObject != null)
        {
            // Dropped on a new inventory slot
            Transform inventorySlot = result.gameObject.transform;
            transform.SetParent(inventorySlot);
            transform.localPosition = Vector3.zero; 
        }
        else if (transform.parent == startParent || transform.parent.GetComponent<InventorySlot>() == null) // Assuming you have an InventorySlot component
        {
            // Not dropped on a new slot, or not an inventory slot; revert to original position
            transform.localPosition = startPosition;
        }
        transform.localScale = originalScale;
    }
    else
    {
        // Dropped outside any slot; revert to original position
        transform.localPosition = startPosition;
    }
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
