using Entities;
using NetworkingCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckSearcher : MonoBehaviour
    {
        private Dictionary<NetworkId, Card> cards = new Dictionary<NetworkId, Card>();
        private string lastFrameSearch = string.Empty;
        private HashSet<string> setCodes;
        private Format currentFormat;

        public bool loading;
        public InputField searchInput;
        public TextMeshProUGUI cardCountText;
        public GameObject Content;
        public GameObject CardPrefab;
        public Dropdown FormatDropDown;

        [Header("Type Toggles")]
        public Toggle pokemonToggle;
        public Toggle trainerToggle;
        public Toggle energyToggle;

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
            FilterCards();
        }

        public void FilterCards()
        {
            var searchString = searchInput.text.Trim().ToLower();

            foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
            {
                if (!IsToggleEnabledForCard(deckCard.card))
                {
                    deckCard.gameObject.SetActive(false);
                    continue;
                }
                else if (deckCard.card is PokemonCard && !IsTypeToggleEnabled(deckCard.card))
                {
                    deckCard.gameObject.SetActive(false);
                    continue;
                }
                else if (!IsSetValid(deckCard.card.SetCode))
                {
                    deckCard.gameObject.SetActive(false);
                    continue;
                }
                else if (!string.IsNullOrEmpty(searchString) && !deckCard.card.Name.ToLower().Contains(searchString))
                {
                    deckCard.gameObject.SetActive(false);
                    continue;
                }

                deckCard.gameObject.SetActive(true);
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

        private bool IsTypeToggleEnabled(Card card)
        {
            switch (((PokemonCard)card).Type)
            {
                case EnergyTypes.Colorless:
                    return ColorlessToggle.isOn;
                case EnergyTypes.Water:
                    return WaterToggle.isOn;
                case EnergyTypes.Fire:
                    return FireToggle.isOn;
                case EnergyTypes.Grass:
                    return GrassToggle.isOn;
                case EnergyTypes.Electric:
                    return ElectricToggle.isOn;
                case EnergyTypes.Psychic:
                    return PsychicToggle.isOn;
                case EnergyTypes.Fighting:
                    return FightingToggle.isOn;
                case EnergyTypes.Darkness:
                    return DarknessToggle.isOn;
                case EnergyTypes.Steel:
                    return SteelToggle.isOn;
                case EnergyTypes.Fairy:
                    return FairyToggle.isOn;
                case EnergyTypes.Dragon:
                    return DragonToggle.isOn;
                default:
                    return true;
            }
        }

        private void Update()
        {
            if (loading)
            {
                return;
            }

            if (currentFormat == null)
            {
                currentFormat = MainMenu.MainMenu.formats.FirstOrDefault(f => f.Name == "Unlimited");
            }

            var searchString = searchInput.text;
            setCodes = new HashSet<string>(currentFormat.Sets.Select(x => x.SetCode));

            if (string.IsNullOrWhiteSpace(searchString.Trim()) || searchString.Trim() == lastFrameSearch)
            {
                return;
            }

            FilterCards();

            lastFrameSearch = searchString;
        }

        public void OnSelectedFormatValueChanged()
        {
            var selected = FormatDropDown.options[FormatDropDown.value].text;
            var format = MainMenu.MainMenu.formats.First(x => x.Name == selected);

            currentFormat = format;
            setCodes = new HashSet<string>(format.Sets.Select(x => x.SetCode));

            FilterCards();
        }

        private bool IsSetValid(string targetCode)
        {
            if (setCodes.Count == 0)
            {
                return true;
            }

            return setCodes.Contains(targetCode);
        }

        IEnumerator LoadCards()
        {
            loading = true;
            int i = 0;

            foreach (var card in CardLoader.LoadAllCards())
            {
                if (card.IgnoreInBuilder)
                {
                    continue;
                }

                cards.Add(card.CardId, card);

                cardCountText.text = cards.Count.ToString();
                var spawnedObject = Instantiate(CardPrefab, Content.transform);
                var deckCard = spawnedObject.GetComponent<DeckCard>();
                deckCard.card = card;
                deckCard.InitAndLoad(card);

                i++;

                if (i > 20)
                {
                    i = 0;
                    yield return new WaitForEndOfFrame();
                }
            }

            loading = false;
        }
    }
}
