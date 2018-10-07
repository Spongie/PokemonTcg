using System.Collections;
using System.Collections.Generic;
using System.IO;
using TCGCards;
using TestCards;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code
{
    public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float DefaultScale;
        public float HoverScale;
        public Card card;
        public Sprite CardBack;
        public bool opponentCard;
        public bool inHand;
        private float defaultY;
        RectTransform rectTransform;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual bool ActivatePointerEnterOrExit() => !opponentCard;

        protected virtual bool RenderBackSide() => opponentCard && inHand;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!ActivatePointerEnterOrExit())
                return;

            defaultY = transform.position.y;
            transform.localScale = new Vector3(HoverScale, HoverScale, 1);
            GetComponent<Canvas>().sortingOrder = 10;
            rectTransform.SetPositionAndRotation(new Vector3(rectTransform.position.x, 0, rectTransform.position.z), rectTransform.rotation);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!ActivatePointerEnterOrExit())
                return;

            transform.localScale = new Vector3(DefaultScale, DefaultScale, 1);
            GetComponent<Canvas>().sortingOrder = 1;
            rectTransform.SetPositionAndRotation(new Vector3(rectTransform.position.x, defaultY, rectTransform.position.z), rectTransform.rotation);
        }

        public void ReloadImage()
        {
            StartCoroutine(LoadSprite());
        }

        IEnumerator LoadSprite()
        {
            if (RenderBackSide())
            {
                GetComponent<Image>().sprite = CardBack;
            }
            else
            {
                card = new TestEkans(null);
                string finalPath = "file://" + Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".jpg";
                var localFile = new WWW(finalPath);

                yield return localFile;

                var texture = localFile.texture;

                GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
        }
    }
}