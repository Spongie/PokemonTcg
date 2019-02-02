using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveDropZone : MonoBehaviour, IDropHandler
{
    public GameObject dropTarget;

    public void OnDrop(PointerEventData eventData)
    {
        var draggedObject = eventData.pointerDrag;
        draggedObject.transform.SetParent(dropTarget.transform);
        var rect = draggedObject.GetComponent<RectTransform>();
        rect.localRotation = Quaternion.identity;
        rect.localPosition = new Vector3(0, 0, 1);
        rect.localScale = new Vector3(1, 1, 1);

        draggedObject.GetComponent<CardZoomer>().enabled = false;
        draggedObject.GetComponent<CardDragger>().enabled = false;
    }
}
