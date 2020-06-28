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

        private List<Card> cards = new List<Card>();
        private string lastFrameSearch;

        private void Start()
        {
            StartCoroutine(LoadCards());
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
                    deckCard.gameObject.SetActive(true);
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
                    deckCard.gameObject.SetActive(true);
                }
            }
        }

        IEnumerator LoadCards()
        {
            loading = true;
            int counter = 0;

            foreach (var assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                foreach (var type in Assembly.Load(assembly).DefinedTypes.Where(type => typeof(Card).GetTypeInfo().IsAssignableFrom(type.AsType()) && !type.IsAbstract && type.Name != nameof(PokemonCard)))
                {
                    var card = Card.CreateFromTypeInfo(type);

                    if (card == null || card.IgnoreInBuilder)
                    {
                        continue;   
                    }

                    cards.Add(card);

                    text.text = cards.Count.ToString();
                    var spawnedObject = Instantiate(CardPrefab, Content.transform);
                    spawnedObject.GetComponent<DeckCard>().Init(card);

                    counter++;

                    if (counter >= 30)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

                yield return new WaitForEndOfFrame();
            }

            loading = false;
        }
    }
}
