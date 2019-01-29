using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code._2D
{
    public class CardDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public bool dragging;

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragging = true;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            GetComponent<RectTransform>().position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragging = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            LayoutRebuilder.MarkLayoutForRebuild(transform.parent.GetComponent<RectTransform>());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            GetComponent<RectTransform>().localPosition = new Vector3(pos.x, 700, pos.z);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            GetComponent<RectTransform>().localPosition = new Vector3(pos.x, 50, pos.z);
        }
    }
}
