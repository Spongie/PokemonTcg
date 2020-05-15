using UnityEngine;
using UnityEngine.EventSystems;

public class CardZoomer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ZoomMode
    {
        FromBottom,
        FromTop,
        Center
    }

    private int oldOrder = 0;
    private Vector2 originalSize;

    public ZoomMode zoomMode;
    
    [SerializeField]
    private bool hovered;
    [SerializeField]
    private bool zooming;

    void Start()
    {
        oldOrder = transform.parent.gameObject.GetComponent<Canvas>().sortingOrder;
        originalSize = GetComponent<RectTransform>().sizeDelta;
    }

    private void Update()
    {
        if (!hovered)
        {
            return;
        }

        if (Input.GetMouseButton(2))
        {
            if (!zooming)
            {
                OnZoomStart();
            }
        }
        else if (zooming)
        {
            OnZoomEnd();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    private void OnZoomStart()
    {
        zooming = true;
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(440, 540);

        if (zoomMode == ZoomMode.FromBottom)
        {
            rectTransform.offsetMax = rectTransform.offsetMax - rectTransform.offsetMin;
            rectTransform.offsetMin = Vector2.zero;
        }
        else if (zoomMode == ZoomMode.FromTop)
        {
            rectTransform.offsetMin = rectTransform.offsetMin - rectTransform.offsetMax;
            rectTransform.offsetMax = Vector2.zero;
        }
        else if (zoomMode == ZoomMode.Center)
        {
            rectTransform.offsetMax = rectTransform.offsetMax - rectTransform.offsetMin;
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x / 2, rectTransform.offsetMax.y / 2);
        }

        transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 100;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        OnZoomEnd();
    }

    private void OnZoomEnd()
    {
        zooming = false;
        var rectTransform = GetComponent<RectTransform>();
        
        rectTransform.sizeDelta = originalSize;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = oldOrder;
    }
}
