using System.Collections;
using System.IO;
using TCGCards;
using TestCards;
using UnityEngine;
using UnityEngine.UI;

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
        public bool isBenched;

        public Transform leftPoint;
        public Transform rightPoint;
        public Transform bottomPoint;
        public HandController handController;

        public float defaultY;
        public bool stopMovingUp = false;

        public RawImage previewImage;

        private MeshRenderer meshRenderer;
        private bool stopFadeOut;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
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

        protected virtual void OnMouseEnter()
        {
            if (ActivatePointerEnterOrExit())
            {
                if (isBenched)
                {
                    handController.FadeOut();
                    //previewImage.color = new Color(1, 1, 1, 1);
                    //previewImage.texture = meshRenderer.material.mainTexture;
                }
                else
                {
                    transform.localScale = new Vector3(HoverScaleX, HoverScaleY, 1);
                    StartCoroutine(MoveIntoScreen());
                }
            }
        }

        protected virtual void OnMouseExit()
        {
            if (ActivatePointerEnterOrExit())
            {
                if (isBenched)
                {
                    handController.FadeIn();
                    //previewImage.color = new Color(1, 1, 1, 0);
                }
                else
                {
                    transform.localScale = new Vector3(DefaultScaleX, DefaultScaleY, 1);
                    StartCoroutine(MoveOutOfScreen());
                }
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
                else
                {
                    transform.Translate(new Vector3(0, 0.1f, 0));
                    current = Camera.main.WorldToViewportPoint(bottomPoint.position);
                    yield return new WaitForEndOfFrame();
                }
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
                meshRenderer.material.mainTexture = CardBack;
            }
            else
            {
                string finalPath = "file://" + Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".jpg";
                var localFile = new WWW(finalPath);

                yield return localFile;

                var texture = localFile.texture;

                meshRenderer.material.mainTexture = texture;
            }
        }

        public IEnumerator FadeOut()
        {
            while (meshRenderer.material.color.a > 0.1f)
            {
                if (stopFadeOut)
                    break;

                meshRenderer.material.color = new Color(1f, 1f, 1f, meshRenderer.material.color.a - 0.05f);
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator FadeIn()
        {
            stopFadeOut = true;

            while (meshRenderer.material.color.a < 1f)
            {
                meshRenderer.material.color = new Color(1f, 1f, 1f, meshRenderer.material.color.a + 0.05f);
                yield return new WaitForEndOfFrame();
            }

            stopFadeOut = false;
        }
    }
}