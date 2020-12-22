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
        public bool loading;
        public InputField searchInput;
        public TextMeshProUGUI cardCountText;
        public GameObject Content;
        public GameObject CardPrefab;

        public Toggle pokemonToggle;
        public Toggle trainerToggle;
        public Toggle energyToggle;

        private Dictionary<NetworkId, Card> cards = new Dictionary<NetworkId, Card>();
        private string lastFrameSearch = string.Empty;
        public Dropdown FormatDropDown;
        private Format currentFormat;

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
                    bool shouldShow = true;

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

                    shouldShow = shouldShow && pokemonToggle.isOn;

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

            if (currentFormat == null)
            {
                currentFormat = MainMenu.MainMenu.formats.FirstOrDefault(f => f.Name == "Unlimited");
            }

            var searchString = searchInput.text;
            var setCodes = new HashSet<string>(currentFormat.Sets.Select(x => x.SetCode));

            if (string.IsNullOrWhiteSpace(searchString) && searchString.Trim() != lastFrameSearch)
            {
                foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
                {
                    deckCard.gameObject.SetActive(IsSetValid(setCodes, deckCard.card.SetCode) && IsToggleEnabledForCard(deckCard.card));
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
                    deckCard.gameObject.SetActive(IsSetValid(setCodes, deckCard.card.SetCode) && IsToggleEnabledForCard(deckCard.card));
                }
            }
        }

        public void OnSelectedFormatValueChanged()
        {
            var selected = FormatDropDown.options[FormatDropDown.value].text;
            var format = MainMenu.MainMenu.formats.First(x => x.Name == selected);

            currentFormat = format;
            var setCodes = new HashSet<string>(format.Sets.Select(x => x.SetCode));

            foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
            {
                deckCard.gameObject.SetActive(IsSetValid(setCodes, deckCard.card.SetCode) && IsToggleEnabledForCard(deckCard.card));
            }
        }

        private bool IsSetValid(HashSet<string> setCodes, string targetCode)
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
