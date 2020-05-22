using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckCard : MonoBehaviour
    {
        public Image art;
        public Card card;

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

        IEnumerator LoadSprite(Card card)
        {
            yield return new WaitForFixedUpdate();

            string fullCardPath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".png";
            string finalPath = "file:///" + fullCardPath;

            using (var request = UnityWebRequestTexture.GetTexture(finalPath))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Error fetching texture");
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                art.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
