using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckBuilder : MonoBehaviour
    {
        public InputField deckName;

        public void OnExitClick()
        {
            //SceneManager.LoadScene
        }

        public void OnSaveClick()
        {
            var filename = deckName.text + ".dck";

            foreach (var character in Path.GetInvalidFileNameChars())
            {
                filename.Replace(character, '\0');
            }

            var fullPath = Path.Combine(Application.streamingAssetsPath, "Decks", filename);

            var data = "";

            File.WriteAllText(fullPath, data);        
        }
    }
}
