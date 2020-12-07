using Assets.Code._2D;
using System;
using System.Collections;
using System.IO;
using TCGCards;
using TCGCards.Core;
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
                deckBuilder.AddToDeck(card.Clone());
            }
            else
            {
                Destroy(gameObject);
                deckBuilder.UpdateCountInfo();
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
                var imagePath = Path.Combine(Application.streamingAssetsPath, "Cards", card.SetCode, card.GetImageName());

                if (SpriteCache.Instance.cache.ContainsKey(imagePath))
                {
                    art.sprite = SpriteCache.Instance.cache[imagePath];
                }

                if (imagePath == null)
                {
                    yield return new WaitForSeconds(0.05f);
                    continue;
                }

                yield return new WaitForSeconds(0.05f);
                try
                {

                    var imageBytes = File.ReadAllBytes(imagePath);
                    var texture = new Texture2D(256, 256);
                    texture.LoadImage(imageBytes);

                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
                    art.sprite = sprite;

                    if (!SpriteCache.Instance.cache.ContainsKey(imagePath))
                    {
                        SpriteCache.Instance.cache.Add(imagePath, sprite);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                break;
            }
        }
    }
}
