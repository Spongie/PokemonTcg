using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class StadiumCardPlayedEventHandler : MonoBehaviour
    {
        public CardRenderer stadiumGameCard;

        public void Trigger(StadiumCardPlayedEvent stadiumEvent)
        {
            if (stadiumGameCard.card != null)
            {
                var player = stadiumGameCard.card.Owner.Id.Equals(GameController.Instance.myId) ? GameController.Instance.Player : GameController.Instance.OpponentPlayer;
                player.DiscardPile.Add(stadiumEvent.Card);
            }

            stadiumGameCard.SetCard(stadiumEvent.Card, false, false);
            var rectTransform = stadiumGameCard.GetComponent<RectTransform>();

            if (stadiumEvent.Card.Owner.Id.Equals(GameController.Instance.myId))
            {
                GameController.Instance.playerHand.RemoveCard(stadiumEvent.Card);
            }

            rectTransform.localScale = new Vector3(2, 2, 2);
            rectTransform.LeanScale(new Vector3(1, 1, 1), 0.35f).setOnComplete(() =>
            {
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
