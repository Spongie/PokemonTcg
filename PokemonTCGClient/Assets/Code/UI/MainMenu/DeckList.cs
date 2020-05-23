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

        private void Start()
        {
            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            foreach (var file in Directory.GetFiles(directory).Where(f => !f.EndsWith(".meta")))
            {
                var spawned = Instantiate(deckPrefab, content.transform);
                spawned.GetComponent<Deck>().Init(new FileInfo(file).Name.Replace(".dck", string.Empty));
            }
        }

        public void OnNewDeckClicked()
        {
            SceneManager.LoadScene("DeckBuilder");
        }
    }
}
