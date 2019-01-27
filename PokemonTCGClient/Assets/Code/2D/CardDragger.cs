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
    public class CardDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    }
}
