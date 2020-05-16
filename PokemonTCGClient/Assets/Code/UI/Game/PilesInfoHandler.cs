using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class PilesInfoHandler : MonoBehaviour
    {
        public bool trackingOpponent;
        public Text cardsInHandText;
        public Text cardsInDeckText;
        public Text cardsInDiscardText;
        public Text cardsInPrizeText;
        public GameObject selectFromListPanel;

        private void Update()
        {
            var player = trackingOpponent ? GameController.Instance.OpponentPlayer : GameController.Instance.Player;

            if (player == null)
            {
                return;
            }

            cardsInHandText.text = player.Hand.Count.ToString();
            cardsInDiscardText.text = player.DiscardPile.Count.ToString();
            cardsInDeckText.text = player.Deck.Cards.Count.ToString();
            cardsInPrizeText.text = player.PrizeCards.Count.ToString();
        }

        public void OnDisplayDiscardPileClick()
        {
            var player = trackingOpponent ? GameController.Instance.OpponentPlayer : GameController.Instance.Player;

            if (player == null)
            {
                return;
            }

            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().InitView(player.DiscardPile);
        }

        public void OnDisplayPricePileClick()
        {
            var player = trackingOpponent ? GameController.Instance.OpponentPlayer : GameController.Instance.Player;

            if (player == null)
            {
                return;
            }

            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().InitView(player.PrizeCards);
        }
    }
}
