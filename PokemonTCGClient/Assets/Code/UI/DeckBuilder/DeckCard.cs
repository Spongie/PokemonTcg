﻿using System.Collections;
using System.IO;
using TCGCards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckCard : MonoBehaviour, IPointerClickHandler
    {
        public Image art;
        public Card card;
        public DeckBuilder deckBuilder;
        public bool isInDeck;

        void Start()
        {
            deckBuilder = GameObject.FindGameObjectWithTag("deckBuilder").GetComponent<DeckBuilder>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (!isInDeck)
            {
                deckBuilder.AddToDeck(card);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        internal void Init(Card card)
        {
            this.card = card;
            StartCoroutine(GetAndSetCardArt(card));
        }

        IEnumerator GetAndSetCardArt(Card card)
        {
            while (true)
            {
                var imagePath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".png";

                if (imagePath == null)
                {
                    yield return new WaitForSeconds(0.05f);
                    continue;
                }

                yield return new WaitForSeconds(0.05f);

                var imageBytes = File.ReadAllBytes(imagePath);
                var texture = new Texture2D(256, 256);
                texture.LoadImage(imageBytes);

                art.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);

                break;
            }
        }
    }
}
