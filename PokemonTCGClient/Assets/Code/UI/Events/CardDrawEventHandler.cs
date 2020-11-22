using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Events
{
    public class CardDrawEventHandler : MonoBehaviour
    {
        public GameObject cardPrefab;
        public Sprite CardBack;

        public void TriggerCardsDrawn(List<Card> cards)
        {
            StartCoroutine(DrawCardsEnumerator(cards));
        }

        IEnumerator DrawCardsEnumerator(List<Card> cards)
        {
            foreach (var card in cards)
            {
                var gameObject = Instantiate(cardPrefab, transform);
                var rectTransform = gameObject.GetComponent<RectTransform>();
                var image = gameObject.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                bool isMyCards = card.Owner.Id.Equals(GameController.Instance.Player.Id);

                if (isMyCards)
                {
                    yield return CardImageLoader.Instance.LoadSpriteRoutine(card, image);
                }
                else
                {
                    image.sprite = CardBack;
                }

                rectTransform.localPosition = new Vector2(800, 0);
                rectTransform.localScale = new Vector2(0.05f, 0.05f);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

                while (true)
                {
                    bool anyChanged = false;
                    if (rectTransform.localScale.x < 1)
                    {
                        rectTransform.localScale = new Vector2(rectTransform.localScale.x + 0.025f, rectTransform.localScale.y + 0.025f);
                        anyChanged = true;
                    }

                    if (rectTransform.localPosition.x > 0)
                    {
                        rectTransform.localPosition = new Vector2(rectTransform.localPosition.x - 18f, 0);
                        anyChanged = true;
                    }

                    if (!anyChanged)
                    {
                        break;
                    }

                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(0.5f);

                var startPos = rectTransform.localPosition;
                var targetPos = isMyCards ? new Vector2(-1000, -540) : new Vector2(-800, 380);

                var points = new Vector3[]
                {
                    startPos,
                    new Vector3(startPos.x - 200, startPos.y),
                    new Vector3(startPos.x - 400, startPos.y),
                    targetPos
                };

                gameObject.LeanScale(new Vector3(0.1f, 0.1f, 0.1f), 0.75f);
                gameObject.LeanMoveLocal(new LTBezierPath(points), 1.0f).destroyOnComplete = true;

                if (isMyCards)
                {
                    GameController.Instance.playerHand.AddCardToHand(card);
                }

                yield return new WaitForSeconds(0.4f);
            }

            yield return new WaitForSeconds(0.6f);
            GameEventHandler.Instance.EventCompleted();
        }
    }
}
