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
        public Sprite cardBack;

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
            string fullCardPath = Path.Combine(Application.streamingAssetsPath, "Cards", card.SetCode, card.GetImageName());
            string finalPath = "file:///" + fullCardPath;

            if (!card.IsRevealedToMe(GameController.Instance.myId))
            {
                if (targetImage == null)
                {
                    yield break;
                }

                targetImage.sprite = cardBack;
            }
            else if (SpriteCache.Instance.cache.ContainsKey(fullCardPath))
            {
                if (targetImage == null)
                {
                    yield break;
                }

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

                    if (targetImage != null)
                    {
                        targetImage.sprite = sprite;
                    }
                    if (!SpriteCache.Instance.cache.ContainsKey(fullCardPath))
                    {
                        SpriteCache.Instance.cache.Add(fullCardPath, sprite);
                    }
                }
            }
        }

        public static CardImageLoader Instance { get; private set; }
    }
}
