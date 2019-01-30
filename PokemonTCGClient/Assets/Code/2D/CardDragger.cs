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
        int oldOrder = 0;

        public void OnBeginDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            GetComponent<RectTransform>().position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Drag end, marking " + transform.parent.name + " for rebuild");
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            LayoutRebuilder.MarkLayoutForRebuild(GameObject.FindGameObjectWithTag("PlayerHand").GetComponent<RectTransform>());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 pos = GetComponent<RectTransform>().localPosition;
            GetComponent<RectTransform>().localPosition = new Vector3(pos.x, 875, pos.z);
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
}
