using NetworkingCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TCGCards;
using TCGCards.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckSearcher : MonoBehaviour
    {
        public bool loading;
        public InputField searchInput;
        public Text text;
        public GameObject Content;
        public GameObject CardPrefab;

        public Toggle pokemonToggle;
        public Toggle trainerToggle;
        public Toggle energyToggle;

        private Dictionary<NetworkId, Card> cards = new Dictionary<NetworkId, Card>();
        private string lastFrameSearch = string.Empty;

        [Header("Energy Toggles")]
        public Toggle FightingToggle;
        public Toggle WaterToggle;
        public Toggle FireToggle;
        public Toggle GrassToggle;
        public Toggle PsychicToggle;
        public Toggle ElectricToggle;
        public Toggle ColorlessToggle;
        public Toggle DarknessToggle;
        public Toggle FairyToggle;
        public Toggle DragonToggle;
        public Toggle SteelToggle;

        private void Start()
        {
            StartCoroutine(LoadCards());
        }

        public void OnToggleChanged()
        {
            foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
            {
                if (deckCard.card is EnergyCard)
                {
                    deckCard.gameObject.SetActive(energyToggle.isOn);
                }
                else if (deckCard.card is PokemonCard)
                {
                    bool shouldShow = pokemonToggle.isOn;

                    switch (((PokemonCard)deckCard.card).Type)
                    {
                        case Entities.EnergyTypes.Colorless:
                            shouldShow = ColorlessToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Water:
                            shouldShow = WaterToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Fire:
                            shouldShow = FireToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Grass:
                            shouldShow = GrassToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Electric:
                            shouldShow = ElectricToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Psychic:
                            shouldShow = PsychicToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Fighting:
                            shouldShow = FightingToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Darkness:
                            shouldShow = DarknessToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Steel:
                            shouldShow = SteelToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Fairy:
                            shouldShow = FairyToggle.isOn;
                            break;
                        case Entities.EnergyTypes.Dragon:
                            shouldShow = DragonToggle.isOn;
                            break;
                        default:
                            break;
                    }
                    deckCard.gameObject.SetActive(shouldShow);
                }
                else if (deckCard.card is TrainerCard)
                {
                    deckCard.gameObject.SetActive(trainerToggle.isOn);
                }
            }
        }

        private bool IsToggleEnabledForCard(Card card)
        {
            if (card is EnergyCard)
            {
                return energyToggle.isOn;
            }
            else if (card is PokemonCard)
            {
                return pokemonToggle.isOn;
            }
            else if (card is TrainerCard)
            {
                return trainerToggle.isOn;
            }

            return false;
        }

        private void Update()
        {
            if (loading)
            {
                return;
            }

            var searchString = searchInput.text;

            if (string.IsNullOrWhiteSpace(searchString) && searchString.Trim() != lastFrameSearch)
            {
                foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
                {
                    deckCard.gameObject.SetActive(IsToggleEnabledForCard(deckCard.card));
                }
            }

            if (string.IsNullOrWhiteSpace(searchString) || searchString.Trim() == lastFrameSearch)
            {
                return;
            }

            lastFrameSearch = searchString;
            var formattedSearch = searchString.ToLower();

            foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
            {
                if (!deckCard.card.GetName().ToLower().Contains(formattedSearch))
                {
                    deckCard.gameObject.SetActive(false);
                }
                else
                {
                    deckCard.gameObject.SetActive(IsToggleEnabledForCard(deckCard.card));
                }
            }
        }

        IEnumerator LoadCards()
        {
            loading = true;
            int counter = 0;

            foreach (var card in CardLoader.LoadAllCards())
            {
                if (card.IgnoreInBuilder)
                {
                    continue;
                }

                cards.Add(card.CardId, card);

                text.text = cards.Count.ToString();
                var spawnedObject = Instantiate(CardPrefab, Content.transform);
                spawnedObject.GetComponent<DeckCard>().Init(card);

                counter++;

                if (counter >= 50)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            loading = false;
        }
    }
}
