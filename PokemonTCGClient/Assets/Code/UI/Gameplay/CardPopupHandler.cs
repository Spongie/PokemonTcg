using Assets.Code.UI.Game;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using UnityEngine;

namespace Assets.Code.UI.Gameplay
{
    public class CardPopupHandler : MonoBehaviour
    {
        private Card card;
        public GameObject AddToBenchButton;
        public GameObject AttachToPokemon;
        public GameObject AttackButtonPrefab;
        public GameObject ActivateAbilityButton;

        public void Init(Card card)
        {
            this.card = card;
            var player = GameController.Instance.Player;
            AddToBenchButton.SetActive(card is PokemonCard && player.BenchedPokemon.Count < 6 && player.Hand.Contains(card));

            if (card is PokemonCard)
            {
                if (GameController.Instance.Player.ActivePokemonCard.Id.Equals(card.Id))
                {
                    foreach (var attack in ((PokemonCard)card).Attacks.Reverse<Attack>())
                    {
                        if (!attack.CanBeUsed(GameController.Instance.gameField, card.Owner, GameController.Instance.OpponentPlayer))
                            continue;

                        var attackButton = Instantiate(AttackButtonPrefab, transform);
                        attackButton.GetComponent<AttackButton>().Init(attack);
                        attackButton.gameObject.transform.SetAsFirstSibling();
                    }
                }

                var ability = ((PokemonCard)card).Ability;

                if (ability != null && ability.TriggerType == TriggerType.Activation && ability.CanActivate())
                {
                    ActivateAbilityButton.SetActive(true);
                    var abilityButton = ActivateAbilityButton.GetComponent<AbilityButton>();
                    abilityButton.Init(ability);
                }
                else
                {
                    ActivateAbilityButton.SetActive(false);
                }
            }
            else if (card is EnergyCard)
            {
                //Do nothing
            }
        }

        public void AddToBenchClicked()
        {
            NetworkManager.Instance.gameService.AddToBench(GameController.Instance.myId, new List<PokemonCard> { (PokemonCard)card });
            gameObject.SetActive(false);
        }
    }
}
