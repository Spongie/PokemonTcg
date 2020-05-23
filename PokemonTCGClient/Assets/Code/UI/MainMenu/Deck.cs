using Assets.Code.UI.DeckBuilder;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI.MainMenu
{
    public class Deck : MonoBehaviour
    {
        private string deckName;
        public Text deckNameText;

        public void Init(string deck)
        {
            deckName = deck;
            deckNameText.text = deckName;
        }

        public void OnEditDeckClicked()
        {
            DeckBuilder.DeckBuilder.CurrentDeck = deckName;
            SceneManager.LoadScene("DeckBuilder");
        }
    }
}
