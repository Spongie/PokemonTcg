using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class StadiumCardDestroyedEventHandler : MonoBehaviour
    {
        public CardRenderer stadiumGameCard;

        public void Trigger(StadiumDestroyedEvent stadiumEvent)
        {
            if (stadiumGameCard.card.Owner.Id.Equals(GameController.Instance.myId))
            {
                GameController.Instance.Player.DiscardPile.Add(stadiumGameCard.card);
            }
            else
            {
                GameController.Instance.OpponentPlayer.DiscardPile.Add(stadiumGameCard.card);
            }

            stadiumGameCard.SetCard(null, false, false);
            var rectTransform = stadiumGameCard.GetComponent<RectTransform>();

            rectTransform.LeanAlpha(0.0f, 0.35f).setOnComplete(() =>
            {
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
