using NetworkingCore;
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
                var deck = Serializer.Deserialize<TCGCards.Core.Deck>(File.ReadAllText(file));
                spawned.GetComponent<Deck>().Init(deck);
            }
        }

        public void OnNewDeckClicked()
        {
            SceneManager.LoadScene("DeckBuilder");
        }

        public void DeleteDeck(Deck deck)
        {
            string filename = deck.GetSafeFileName();
            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            File.Delete(Path.Combine(directory, filename + Deck.deckExtension));
            Destroy(deck.gameObject);
        }
    }
}
