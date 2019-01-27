using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code._2D
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        public GameObject dropTarget;

        public void OnDrop(PointerEventData eventData)
        {
            eventData.pointerDrag.transform.SetParent(dropTarget.transform);
            var rect = eventData.pointerDrag.GetComponent<RectTransform>();
            rect.localRotation = Quaternion.identity;
            rect.localPosition = new Vector3(0, 0, 1);
            rect.localScale = new Vector3(1, 1, 1);
        }
    }
}
