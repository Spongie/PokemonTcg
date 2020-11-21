using System.Collections;
using TCGCards.Core.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Events
{
    public class CardDiscardedEventHandler : MonoBehaviour
    {
        public GameObject cardPrefab;
        public Sprite CardBack;

        public void Trigger(CardsDiscardedEvent discardEvent)
        {
            StartCoroutine(DiscardCardsEnumerator(discardEvent));
        }

        IEnumerator DiscardCardsEnumerator(CardsDiscardedEvent discardEvent)
        {
            bool isMe = discardEvent.Player.Equals(GameController.Instance.myId);

            foreach (var card in discardEvent.Cards)
            {
                var gameObject = Instantiate(cardPrefab, transform);
                var rectTransform = gameObject.GetComponent<RectTransform>();
                var image = gameObject.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

                if (isMe)
                {
                    yield return CardImageLoader.Instance.LoadSpriteRoutine(card, image);
                    rectTransform.localScale = new Vector2(1f, 1f);
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                    GameController.Instance.playerHand.RemoveCard(card);
                }
                else
                {
                    image.sprite = CardBack;
                    rectTransform.localScale = new Vector2(1f, 1f);
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

                    gameObject.LeanScaleX(0f, 0.3f);
                    yield return new WaitForSeconds(0.3f);

                    if (!isMe)
                    {
                        yield return CardImageLoader.Instance.LoadSpriteRoutine(card, image);
                    }

                    gameObject.LeanScaleX(1f, 0.3f);
                }

                yield return new WaitForSeconds(0.4f);

                gameObject.LeanMoveLocal(new Vector2(-1400, 0), 0.4f).destroyOnComplete = true;
                gameObject.LeanScale(new Vector3(0.5f, 0.5f, 0.4f), 0.5f);
            }

            GameEventHandler.Instance.EventCompleted();
        }
    }
}
