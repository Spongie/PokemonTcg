using Assets.Code._2D;
using System.Collections;
using System.IO;
using TCGCards;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Code
{
    public class CardImageLoader : MonoBehaviour
    {
        private void Awake()
        {
            Instance = this;    
        }

        public void LoadSprite(Card card, Image targetImage)
        {
            StartCoroutine(LoadSpriteRoutine(card, targetImage));
        }

        public IEnumerator LoadSpriteRoutine(Card card, Image targetImage)
        {
            string fullCardPath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".png";
            string finalPath = "file:///" + fullCardPath;

            if (SpriteCache.Instance.cache.ContainsKey(fullCardPath))
            {
                targetImage.sprite = SpriteCache.Instance.cache[fullCardPath];
            }
            else
            {
                using (var request = UnityWebRequestTexture.GetTexture(finalPath))
                {
                    yield return request.SendWebRequest();

                    if (request.isNetworkError || request.isHttpError)
                    {
                        Debug.LogError("Error fetching texture");
                    }

                    var texture = DownloadHandlerTexture.GetContent(request);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    targetImage.sprite = sprite;

                    SpriteCache.Instance.cache.Add(fullCardPath, sprite);
                }
            }
        }

        public static CardImageLoader Instance { get; private set; }
    }
}
