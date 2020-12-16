using Assets.Code.UI.DeckBuilder;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI.MainMenu
{
    public class Deck : MonoBehaviour
    {
        public const string deckExtension = ".deck";
        public TCGCards.Core.Deck deck;
        public TextMeshProUGUI deckNameText;

        public void Init(TCGCards.Core.Deck deck)
        {
            this.deck = deck;
            var formatName = MainMenu.formats.FirstOrDefault(f => f.Id.Equals(deck.FormatId)).Name;
            deckNameText.text = $"{deck.Name} - {formatName}";
        }

        public void OnEditDeckClicked()
        {
            DeckBuilder.DeckBuilder.CurrentDeck = deck;
            SceneManager.LoadScene("DeckBuilder");
        }

        public void OnDeleteDeckClicked()
        {
            DeckList.Instance.DeleteDeck(this);
        }

        public string GetSafeFileName()
        {
            var name = deck.Name;

            foreach (var character in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(character, '\0');
            }

            return name;
        }
    }
}
