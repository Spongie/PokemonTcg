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
        private List<GameObject> attackButtons;

        public Transform buttonParent;

        [Header("Buttons")]
        public GameObject AddToBenchButton;
        public GameObject AttackButtonPrefab;
        public GameObject ActivateAbilityButton;
        public GameObject EvolveButton;
        public GameObject RetreatButton;

        public void Init(Card card)
        {
            attackButtons = new List<GameObject>();
            this.card = card;
            Player player;

            if (card is PokemonCard)
            {
                player = GameController.Instance.Player ?? new Player() { ActivePokemonCard = (PokemonCard)card };
            }
            else
            {
                player = GameController.Instance.Player ?? new Player();
            }

            AddToBenchButton.SetActive(card is PokemonCard && player.BenchedPokemon.Count < GameField.BenchMaxSize && player.Hand.Contains(card) && ((PokemonCard)card).Stage == 0);

            if (card is PokemonCard)
            {
                var pokemonCard = (PokemonCard)card;

                if (player?.ActivePokemonCard != null && player.ActivePokemonCard.Id.Equals(card.Id))
                {
                    foreach (var attack in pokemonCard.Attacks.Reverse())
                    {
                        var attackButton = Instantiate(AttackButtonPrefab, buttonParent);
                        attackButton.GetComponent<AttackButton>().Init(attack);
                        attackButton.gameObject.transform.SetAsFirstSibling();
                        attackButtons.Add(attackButton);
                    }

                    RetreatButton.SetActive(GameController.Instance.gameField.CanRetreat(pokemonCard));
                }
                else
                {
                    RetreatButton.SetActive(false);
                }

                EvolveButton.SetActive(pokemonCard.Stage >= 1 && player.Hand.Any(x => x.Id.Equals(pokemonCard.Id)));

                var ability = pokemonCard.Ability;

                if (ability != null)
                {
                    ability.PokemonOwner = (PokemonCard)card; //maybe its fixed dont care...Stupid fix because of serialize not assigning it?!
                }

                if (ability != null && ability.TriggerType == TriggerType.Activation && !player.Hand.Contains(card))
                {
                    ActivateAbilityButton.SetActive(true);
                    var abilityButton = ActivateAbilityButton.GetComponent<AbilityButton>();
                    abilityButton.gameObject.transform.SetAsFirstSibling();
                    abilityButton.Init(ability);
                }
                else
                {
                    ActivateAbilityButton.SetActive(false);
                }
            }
            else if (card is TrainerCard)
            {
                var trainerCard = (TrainerCard)card;

                if (trainerCard.Ability == null || trainerCard.Ability.TriggerType != TriggerType.Activation)
                {
                    gameObject.SetActive(false);
                    return;
                }

                ActivateAbilityButton.SetActive(true);
                var abilityButton = ActivateAbilityButton.GetComponent<AbilityButton>();
                abilityButton.gameObject.transform.SetAsFirstSibling();
                abilityButton.Init(trainerCard.Ability);
                EvolveButton.SetActive(false);
                RetreatButton.SetActive(false);
            }
            else
            {
                ActivateAbilityButton.SetActive(false);
                EvolveButton.SetActive(false);
                RetreatButton.SetActive(false);
            }
        }

        public void AddToBenchClicked()
        {
            NetworkManager.Instance.gameService.AddToBench(GameController.Instance.gameField.Id, GameController.Instance.myId, new List<NetworkId> { card.Id });
            gameObject.SetActive(false);

            ClearAttackButtons();
            gameObject.SetActive(false);
        }

        public void CancelClick()
        {
            ClearAttackButtons();
            gameObject.SetActive(false);
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
            Ability ability;

            if (card is PokemonCard)
            {
                ability = ((PokemonCard)card).Ability;
            }
            else
            {
                ability = ((TrainerCard)card).Ability;
            }

            NetworkManager.Instance.gameService.ActivateAbility(GameController.Instance.gameField.Id, ability.Id);
            gameObject.SetActive(false);
            ClearAttackButtons();
        }

        public void RetreatClick()
        {
            ClearAttackButtons();
            gameObject.SetActive(false);
            GameController.Instance.StartRetreating((PokemonCard)card);
        }
    }
}
