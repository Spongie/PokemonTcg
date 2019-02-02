using UnityEngine;
using UnityEngine.EventSystems;

public class BenchDropZone : MonoBehaviour, IDropHandler
{
    public GameObject dropTarget;

    public void OnDrop(PointerEventData eventData)
    {
        var draggedObject = eventData.pointerDrag;
        var parent = draggedObject.transform.parent;
        parent.SetParent(dropTarget.transform);

        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.localRotation = Quaternion.identity;
        parentRect.localPosition = new Vector3(parentRect.anchoredPosition.x, parentRect.anchoredPosition.y, 0);
        parentRect.localScale = new Vector3(1, 1, 1);


        var rect = draggedObject.GetComponent<RectTransform>();
        rect.localRotation = Quaternion.identity;
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = new Vector3(1, 1, 1);

        draggedObject.GetComponent<CardDragger>().OnEndDrag(eventData);
        draggedObject.GetComponent<CardZoomer>().enabled = false;
        draggedObject.GetComponent<CardDragger>().enabled = false;
    }
}
