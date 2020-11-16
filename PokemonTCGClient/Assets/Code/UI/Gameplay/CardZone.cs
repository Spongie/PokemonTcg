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
        public int yOffset = 2;
        public bool overrideHeight = true;
        public bool adjustZoneWidth;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();

            height = (int)rectTransform.rect.height;
        }

        private void Update()
        {
            int offset = 3;
            int ignoreCount = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag == "Ignore")
                {
                    ignoreCount++;
                }
            }

            if (Centered)
            {
                var center = rectTransform.rect.width / 2;
                var totalSize = width * (transform.childCount - ignoreCount);
                offset = (int)(center - (totalSize / 2));
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                if (child.tag == "Ignore")
                {
                    continue;
                }

                var rect = child.GetComponent<RectTransform>();

                var actualHeight = overrideHeight ? height - 5 : rect.sizeDelta.y;
                rect.sizeDelta = new Vector2(width, actualHeight);
                rect.anchoredPosition = new Vector3(offset, yOffset);
                
                offset += width + 3;
            }

            if (adjustZoneWidth)
            {
                rectTransform.sizeDelta = new Vector2(offset, rectTransform.sizeDelta.y);
            }
        }
    }
}
