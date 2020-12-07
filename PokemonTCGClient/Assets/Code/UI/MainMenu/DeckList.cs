using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.UI.MainMenu
{
    public class DeckList : MonoBehaviour
    {
        public GameObject content;
        public GameObject deckPrefab;
        public static DeckList Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            foreach (var file in Directory.GetFiles(directory).Where(f => f.EndsWith(Deck.deckExtension)))
            {
                var spawned = Instantiate(deckPrefab, content.transform);
                spawned.GetComponent<Deck>().Init(new FileInfo(file).Name.Replace(Deck.deckExtension, string.Empty));
            }
        }

        public void OnNewDeckClicked()
        {
            SceneManager.LoadScene("DeckBuilder");
        }

        public void DeleteDeck(Deck deck)
        {
            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            File.Delete(Path.Combine(directory, deck.deckNameText.text + Deck.deckExtension));
            Destroy(deck.gameObject);
        }
    }
}
