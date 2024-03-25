using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    private Vector3 startPosition;
    private Transform startParent;
    private CanvasGroup canvasGroup;

    public GameObject originalSlot;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.localPosition; // Use localPosition for more accurate reset within parent
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

        // Check if the item has been dropped into a new slot or needs to return to its start position
        if (transform.parent == startParent)
        {
            transform.localPosition = startPosition; // Snap back if not dropped into a new slot
        }
        else
        {
            // Reset scale or other properties as needed when successfully dropped into a new slot
            // transform.localScale = Vector3.one;
        }
    }

    public void ResetDraggable()
    {
        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(true);
        transform.SetParent(startParent);
        transform.localPosition = startPosition;
        transform.localScale = Vector3.one; // Reset scale if it was altered during dragging
    }
}
