﻿using System;
using System.IO;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckBuilder : MonoBehaviour
    {
        public InputField deckName;
        public GameObject cardPrefab;
        public GameObject deckContent;

        public void OnExitClick()
        {
            //SceneManager.LoadScene
        }

        public void OnSaveClick()
        {
            var filename = deckName.text + ".dck";

            foreach (var character in Path.GetInvalidFileNameChars())
            {
                filename.Replace(character, '\0');
            }

            var fullPath = Path.Combine(Application.streamingAssetsPath, "Decks", filename);

            var data = "";

            File.WriteAllText(fullPath, data);        
        }

        internal void AddToDeck(Card card)
        {
            var spawnedObject = Instantiate(cardPrefab, deckContent.transform);
            var deckCard = spawnedObject.GetComponent<DeckCard>();

            deckCard.Init(card);
            deckCard.isInDeck = true;

            spawnedObject.GetComponentInChildren<CardZoomer>().zoomMode = ZoomMode.FromTopLeft;
        }
    }
}
