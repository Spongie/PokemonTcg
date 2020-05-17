using System.Collections.Generic;
using TCGCards;
using UnityEngine;

namespace Assets.Code.UI.Gameplay
{
    public class CardPopupHandler : MonoBehaviour
    {
        private Card card;
        public GameObject AddToBenchButton;

        public void Init(Card card)
        {
            this.card = card;
            var player = GameController.Instance.Player;
            AddToBenchButton.SetActive(card is PokemonCard && player.BenchedPokemon.Count < 6 && player.Hand.Contains(card));
        }

        public void AddToBenchClicked()
        {
            NetworkManager.Instance.gameService.AddToBench(GameController.Instance.myId, new List<PokemonCard> { (PokemonCard)card });
            gameObject.SetActive(false);
        }
    }
}
