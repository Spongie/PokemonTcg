using Assets.Code.UI.Game;
using NetworkingCore;
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
        public GameObject EvolveButton;
        public GameObject RetreatButton;
        private List<GameObject> attackButtons;

        public void Init(Card card)
        {
            attackButtons = new List<GameObject>();
            this.card = card;
            var player = GameController.Instance.Player;
            AddToBenchButton.SetActive(card is PokemonCard && player.BenchedPokemon.Count < 6 && player.Hand.Contains(card));

            if (card is PokemonCard)
            {
                var pokemonCard = (PokemonCard)card;

                if (GameController.Instance.Player?.ActivePokemonCard != null && GameController.Instance.Player.ActivePokemonCard.Id.Equals(card.Id))
                {
                    foreach (var attack in ((PokemonCard)card).Attacks.Reverse<Attack>())
                    {
                        if (!attack.CanBeUsed(GameController.Instance.gameField, card.Owner, GameController.Instance.OpponentPlayer))
                            continue;

                        var attackButton = Instantiate(AttackButtonPrefab, transform);
                        attackButton.GetComponent<AttackButton>().Init(attack);
                        attackButton.gameObject.transform.SetAsFirstSibling();
                        attackButtons.Add(attackButton);
                    }

                    RetreatButton.SetActive(pokemonCard.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount) > pokemonCard.RetreatCost);
                }
                else
                {
                    ActivateAbilityButton.SetActive(false);
                }

                EvolveButton.SetActive(pokemonCard.Stage > 0);

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
            else
            {
                EvolveButton.SetActive(false);
                RetreatButton.SetActive(false);
            }
        }

        public void AddToBenchClicked()
        {
            NetworkManager.Instance.gameService.AddToBench(GameController.Instance.gameField.Id, GameController.Instance.myId, new List<NetworkId> { card.Id });
            gameObject.SetActive(false);

            ClearAttackButtons();
        }

        public void ClearAttackButtons()
        {
            foreach (var attackButton in attackButtons)
            {
                Destroy(attackButton);
            }
        }

        public void OnEvolveClick()
        {
            GameController.Instance.StartEvolving((PokemonCard)card);
            gameObject.SetActive(false);
            ClearAttackButtons();
        }

        public void OnAbilityClick()
        {
            NetworkManager.Instance.gameService.ActivateAbility(GameController.Instance.gameField.Id, ((PokemonCard)card).Ability.Id);
            gameObject.SetActive(false);
            ClearAttackButtons();
        }

        public void RetreatClick()
        {
            gameObject.SetActive(false);
            GameController.Instance.StartRetreating(card);
        }
    }
}
