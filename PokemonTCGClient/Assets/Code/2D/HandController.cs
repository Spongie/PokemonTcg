﻿using Assets.Code._2D.GameCard;
using NetworkingCore;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using UnityEngine;

namespace Assets.Code._2D
{
    public class HandController : MonoBehaviour
    {
        public GameObject cardPrefab;
        public GameObject cardZone;
        public bool isPlayerHand;
        private Dictionary<NetworkId, GameObject> idObjectCache;

        private void Awake()
        {
            idObjectCache = new Dictionary<NetworkId, GameObject>();
        }

        public void SetHand(IEnumerable<Card> cards)
        {
            idObjectCache.Clear();

            for (int i = 0; i < cardZone.transform.childCount; i++)
            {
                Destroy(cardZone.transform.GetChild(i).gameObject);
            }

            int index = 0;
            foreach (var card in cards)
            {
                if (isPlayerHand)
                {
                    card.RevealToAll();
                }

                var spawnedCard = Instantiate(cardPrefab, cardZone.transform);
                spawnedCard.GetComponentInChildren<CardRenderer>().SetCard(card, false);
                spawnedCard.GetComponent<Zoomer>().SetPivotForHand();
                index++;
                idObjectCache.Add(card.Id, spawnedCard);
                GameController.Instance.AddCard(spawnedCard.GetComponent<CardRenderer>());
            }
        }

        internal void RemoveCardById(NetworkId pokemonId)
        {
            if (!idObjectCache.ContainsKey(pokemonId))
            {
                return;
            }

            Destroy(idObjectCache[pokemonId]);
        }

        public void AddCardToHand(Card card)
        {
            if (isPlayerHand)
            {
                card.RevealToAll();
            }

            var spawnedCard = Instantiate(cardPrefab, cardZone.transform);
            spawnedCard.GetComponentInChildren<CardRenderer>().SetCard(card, false);
            idObjectCache.Add(card.Id, spawnedCard);
            GameController.Instance.AddCard(spawnedCard.GetComponent<CardRenderer>());
            spawnedCard.GetComponent<Zoomer>().SetPivotForHand();
        }

        public void FadeOutCards(IEnumerable<Card> cards)
        {
            for (int i = 0; i < cardZone.transform.childCount; i++)
            {
                var cardRenderer = cardZone.transform.GetChild(i).GetComponentInChildren<CardRenderer>();
                CanvasGroup cardCanvas = cardRenderer.GetComponent<CanvasGroup>();
                cardCanvas.alpha = cards.Any(card => card.Id.Equals(cardRenderer.card.Id)) ? 0.2f : 1f;
            }
        }

        internal void RemoveCard(Card card)
        {
            if (!idObjectCache.ContainsKey(card.Id))
            {
                return;
            }

            GameController.Instance.Player.Hand.Remove(card);

            Destroy(idObjectCache[card.Id]);
            idObjectCache.Remove(card.Id);
        }
    }
}
