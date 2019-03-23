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

        IEnumerator LoadSpriteRoutine(Card card, Image targetImage)
        {
            string fullCardPath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".jpg";
            string finalPath = "file:///" + fullCardPath;

            using (var request = UnityWebRequestTexture.GetTexture(finalPath))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Error fetching texture");
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }

        public static CardImageLoader Instance { get; private set; }
    }
}
