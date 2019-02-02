using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        GlobalStateHandler.Instance.isDragging = true;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GlobalStateHandler.Instance.isDragging = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        LayoutRebuilder.MarkLayoutForRebuild(GameObject.FindGameObjectWithTag("PlayerHand").GetComponent<RectTransform>());
    }
}
