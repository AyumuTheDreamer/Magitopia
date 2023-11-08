using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    public Item itemData; // The Item data that this draggable item represents.

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;

        // Make the item semi-transparent while dragging
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the draggable item to follow the mouse cursor
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the item's appearance and position
        transform.position = originalPosition;
        transform.SetParent(originalParent);
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // TODO: Check if the item was dropped over the destination container and handle accordingly
    }
}
