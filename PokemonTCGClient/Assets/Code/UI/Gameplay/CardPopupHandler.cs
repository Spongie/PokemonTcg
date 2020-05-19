using System.Collections.Generic;
using TCGCards;
using UnityEngine;

namespace Assets.Code.UI.Gameplay
{
    public class CardPopupHandler : MonoBehaviour
    {
        private Card card;
        public GameObject AddToBenchButton;
        public GameObject AttackButtonPrefab;

        public void Init(Card card)
        {
            this.card = card;
            var player = GameController.Instance.Player;
            AddToBenchButton.SetActive(card is PokemonCard && player.BenchedPokemon.Count < 6 && player.Hand.Contains(card));

            if (card is PokemonCard && GameController.Instance.Player.ActivePokemonCard.Id.Equals(card.Id))
            {
                foreach (var attack in ((PokemonCard)card).Attacks)
                {
                    if (!attack.CanBeUsed(GameController.Instance.gameField, card.Owner, GameController.Instance.OpponentPlayer))
                        continue;

                    var attackButton = Instantiate(AttackButtonPrefab, transform);
                    attackButton.GetComponent<AttackButton>().Init(attack);
                }
            }
        }

        public void AddToBenchClicked()
        {
            NetworkManager.Instance.gameService.AddToBench(GameController.Instance.myId, new List<PokemonCard> { (PokemonCard)card });
            gameObject.SetActive(false);
        }
    }
}
