using UnityEngine;
using UnityEngine.EventSystems;

class CardZoomer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int oldOrder = 0;

    void Start()
    {
        oldOrder = transform.parent.gameObject.GetComponent<Canvas>().sortingOrder;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GlobalStateHandler.Instance.isDragging)
        {
            return;
        }

        Vector3 pos = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition = new Vector3(pos.x, 650, pos.z);
        oldOrder = transform.parent.gameObject.GetComponent<Canvas>().sortingOrder;
        transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 100;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Vector3 pos = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition = new Vector3(pos.x, 50, pos.z);
        transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = oldOrder;
    }
}
