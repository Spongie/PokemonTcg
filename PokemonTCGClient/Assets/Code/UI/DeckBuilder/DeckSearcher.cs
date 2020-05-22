﻿using System.Collections;
using System.Collections.Generic;
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

            if (string.IsNullOrWhiteSpace(searchString) || searchString.Trim() == lastFrameSearch)
            {
                return;
            }

            lastFrameSearch = searchString;

            foreach (var deckCard in Content.GetComponentsInChildren<DeckCard>(true))
            {
                if (!deckCard.card.GetName().Contains(searchString))
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

            foreach (var typeInfo in Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).SelectMany(x => x.DefinedTypes)
                .Where(type => typeof(Card).GetTypeInfo().IsAssignableFrom(type.AsType()) && !type.IsAbstract && type.Name != nameof(PokemonCard)))
            {
                var constructor = typeInfo.DeclaredConstructors.First();
                var parameters = new List<object>();

                for (int i = 0; i < constructor.GetParameters().Length; i++)
                {
                    parameters.Add(null);
                }

                var card = (Card)constructor.Invoke(parameters.ToArray());
                if (card.IsTestCard)
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

            loading = false;
        }
    }
}
