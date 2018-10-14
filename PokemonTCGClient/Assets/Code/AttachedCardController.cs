using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    public class AttachedCardController : CardController, IPointerDownHandler, IPointerUpHandler
    {
        public int originalSortOrder;

        protected override bool ActivatePointerEnterOrExit() => false;

        public override void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Canvas>().sortingOrder = originalSortOrder;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Middle)
                return;


            Canvas canvas = GetComponent<Canvas>();
            originalSortOrder = canvas.sortingOrder;
            canvas.sortingOrder = 101;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Middle)
                return; 

            GetComponent<Canvas>().sortingOrder = originalSortOrder;
        }
    }
}