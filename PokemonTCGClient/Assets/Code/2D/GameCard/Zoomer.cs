using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code._2D.GameCard
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    public class Zoomer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private const int cBottomLeftIndex = 0;
        private const int cTopLeftIndex = 1;
        private const int cTopRightIndex = 2;
        private const int cBottomRightIndex = 3;

        private RectTransform rectTransform;
        private Canvas canvas;
        private int originalSortorder;
        private Vector2 oldPivot;
        [SerializeField]private bool isZooming;
        [SerializeField]private bool isHovered;
        [SerializeField] private bool skipWorldConversion;

        public Vector3 zoomedScale = new Vector3(3.5f, 3.5f, 1f);

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            originalSortorder = canvas.sortingOrder;
            oldPivot = rectTransform.pivot;
        }

        private void Update()
        {
            if (!isHovered)
            {
                return;
            }

            if (Input.GetMouseButton(2))
            {
                if (!isZooming)
                {
                    //BeginZoom();
                }
            }
            else if (isZooming)
            {
                EndZoom();
            }
        }

        private void BeginZoom()
        {
            isZooming = true;
            canvas.sortingOrder = 999;
            oldPivot = rectTransform.pivot;

            SetZoomPivot();

            rectTransform.LeanScale(zoomedScale, 0.15f);
        }

        private Vector3 GetPoint(Vector3 basePoint)
        {
            if (skipWorldConversion)
            {
                return basePoint;
            }

            return Camera.main.WorldToScreenPoint(basePoint);
        }

        private void SetZoomPivot()
        {
            rectTransform.localScale = zoomedScale;
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var screenRect = new Rect(0, 0, Screen.width, Screen.height);

            if (!screenRect.Contains(GetPoint(corners[cTopLeftIndex])))
            {
                if (!screenRect.Contains(GetPoint(corners[cTopRightIndex])))
                {
                    rectTransform.pivot = new Vector2(0.5f, 1);

                    if (!screenRect.Contains(GetPoint(corners[cBottomLeftIndex])))
                    {
                        rectTransform.pivot = new Vector2(0, 1);
                    }
                    else if (!screenRect.Contains(GetPoint(corners[cBottomRightIndex])))
                    {
                        rectTransform.pivot = new Vector2(1, 1);
                    }
                }
                else
                {
                    rectTransform.pivot = new Vector2(0, 0);
                }
            }
            else if (!screenRect.Contains(GetPoint(corners[cTopRightIndex])))
            {
                if (!screenRect.Contains(GetPoint(corners[cBottomRightIndex])))
                {
                    rectTransform.pivot = new Vector2(1, 1);

                    if (!screenRect.Contains(GetPoint(corners[cBottomLeftIndex])))
                    {
                        rectTransform.pivot = new Vector2(1, 0f);
                    }
                }
                else
                {
                    rectTransform.pivot = new Vector2(1, 0);
                }
            }
            else if (!screenRect.Contains(GetPoint(corners[cBottomLeftIndex])))
            {
                rectTransform.pivot = new Vector2(0.5f, 0);
            }
            else if (!screenRect.Contains(GetPoint(corners[cBottomRightIndex])))
            {
                rectTransform.pivot = new Vector2(0.5f, 0);
            }
            else
            {
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }

            rectTransform.localScale = new Vector3(1, 1, 1);
        }

        private void EndZoom()
        {
            isZooming = false;
            rectTransform.LeanScale(new Vector3(1, 1, 1), 0.1f).setOnComplete(() =>
              {
                  canvas.sortingOrder = originalSortorder;
                  rectTransform.pivot = oldPivot;
              });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter != gameObject)
            {
                return;
            }    

            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            EndZoom();
        }

        public void SetPivotForHand()
        {
            rectTransform.pivot = new Vector2(0.5f, 0);
        }

        public void SetPivotForGame()
        {
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                BeginZoom();
            }
        }
    }
}
