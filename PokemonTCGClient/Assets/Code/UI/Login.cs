using System.Collections;
using System.IO;
using System.Linq;
using Assets.Code;
using Boo.Lang;
using NetworkingCore;
using TCGCards;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public RectTransform progressImage;
    public TextMeshProUGUI LoadPercentText;
    public GameObject progressPanel;
    public GameObject LoginPanel;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        StartCoroutine(CacheDecks());
    }

    public void LoginClick()
    {
        NetworkManager.Instance.Me.Name = usernameInput.text;
        NetworkManager.Instance.playerService.Login(usernameInput.text, NetworkManager.Instance.Me.Id);

        SceneManager.LoadScene("MainMenu");
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    public IEnumerator CacheDecks()
    {
        var directory = Path.Combine(Application.streamingAssetsPath, "Decks");
        var cardsToLoad = new List<Card>();
        int cardsLoaded = 0;

        foreach (var file in Directory.GetFiles(directory).Where(f => f.EndsWith(Assets.Code.UI.MainMenu.Deck.deckExtension)))
        {
            var deck = Serializer.Deserialize<TCGCards.Core.Deck>(File.ReadAllText(file));
            
            foreach (var card in deck.Cards)
            {
                cardsToLoad.Add(card);
            }
        }

        foreach (var card in cardsToLoad)
        {
            yield return CardImageLoader.Instance.LoadSpriteRoutine(card, null);
            cardsLoaded++;
            var loadPercent = cardsLoaded / (float)cardsToLoad.Count;
            progressImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500 * loadPercent);
            LoadPercentText.text = (loadPercent * 100).ToString("F2") + "%";
        }

        progressPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
}
