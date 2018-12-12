using System.Collections;
using System.IO;
using TCGCards;
using TestCards;
using UnityEngine;

namespace Assets.Code
{
    public class CardController : MonoBehaviour
    {
        public float DefaultScaleX;
        public float DefaultScaleY;
        public float HoverScaleX;
        public float HoverScaleY;
        public Card card;
        public Texture CardBack;
        public bool opponentCard;
        public bool inHand;

        public Transform leftPoint;
        public Transform rightPoint;
        public Transform bottomPoint;

        public float defaultY;
        
        public bool stopMovingUp = false;

        void Start()
        {
            defaultY = transform.position.y;
            SetCard(new TestEkans(null));
        }

        public virtual void SetCard(Card card)
        {
            this.card = card;
            ReloadImage();
        }

        protected virtual bool ActivatePointerEnterOrExit() => !opponentCard;

        protected virtual bool RenderBackSide() => opponentCard && inHand;

        private void OnMouseEnter()
        {
            if (ActivatePointerEnterOrExit())
            {
                transform.localScale = new Vector3(HoverScaleX, HoverScaleY, 1);
                StartCoroutine(MoveIntoScreen());
            }
        }

        private void OnMouseExit()
        {
            if (ActivatePointerEnterOrExit())
            {
                transform.localScale = new Vector3(DefaultScaleX, DefaultScaleY, 1);
                StartCoroutine(MoveOutOfScreen());
            }
        }

        IEnumerator MoveIntoScreen()
        {
            stopMovingUp = false;

            Vector3 current = Camera.main.WorldToViewportPoint(bottomPoint.position);

            while (current.y < 0)
            {
                if (stopMovingUp)
                    break;

                transform.Translate(new Vector3(0, 0.1f, 0));
                current = Camera.main.WorldToViewportPoint(bottomPoint.position);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator MoveOutOfScreen()
        {
            stopMovingUp = true;

            while (transform.position.y > defaultY)
            {
                transform.Translate(new Vector3(0, -0.1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }

        public void ReloadImage()
        {
            StartCoroutine(LoadSprite());
        }

        IEnumerator LoadSprite()
        {
            if (RenderBackSide())
            {
                GetComponent<MeshRenderer>().material.mainTexture = CardBack;
            }
            else
            {
                string finalPath = "file://" + Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".jpg";
                var localFile = new WWW(finalPath);

                yield return localFile;

                var texture = localFile.texture;

                GetComponent<MeshRenderer>().material.mainTexture = texture;
            }
        }
    }
}