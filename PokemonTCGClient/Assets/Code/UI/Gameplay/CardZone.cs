using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

namespace Assets.Code.UI.Gameplay
{
    public class CardZone : MonoBehaviour
    {
        private RectTransform rectTransform;
        public bool Centered;
        public int maxCards = 0;
        public int width = 200;
        public int height = 200;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();

            height = (int)rectTransform.rect.height;
        }

        private void Update()
        {
            int offset = 3;

            if (Centered)
            {
                var center = rectTransform.rect.width / 2;
                var totalSize = width * transform.childCount;
                offset = (int)(center - (totalSize / 2));
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                var rect = transform.GetChild(i).GetComponent<RectTransform>();

                rect.sizeDelta = new Vector2(width, height - 5);
                rect.localPosition = new Vector3(offset, 2);
                offset += width + 3;
            }
        }
    }
}
