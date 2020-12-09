using TCGCards.Core.GameEvents;
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

            cardsInDiscardText.text = player.DiscardPile.Count.ToString();
            cardsInPrizeText.text = player.PrizeCards.Count.ToString();
        }

        public void UpdateWithInfo(PlayerInfo playerInfo)
        {
            cardsInHandText.text = playerInfo.CardsInHand.ToString();
            cardsInDeckText.text = playerInfo.CardsInDeck.ToString();
        }

        public void OnDisplayDiscardPileClick()
        {
            if (GameController.Instance.SpecialState != SpecialGameState.None)
            {
                return;
            }

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
            if (GameController.Instance.SpecialState != SpecialGameState.None)
            {
                return;
            }

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
