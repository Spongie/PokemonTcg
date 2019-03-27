using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class SelectFromListPanel : MonoBehaviour
    {
        public GameObject previewCard;
        private List<Card> selectedCards;
        private int limit;
        private int minCount;

        public void Init(PickFromListMessage pickFromListMessage)
        {
            limit = pickFromListMessage.MaxCount;
            minCount = pickFromListMessage.MinCount;

            foreach (var card in pickFromListMessage.PossibleChoices)
            {
                GameObject preview = Instantiate(previewCard, transform);
                CardImageLoader.Instance.LoadSprite(card, preview.GetComponent<Image>());
                preview.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnCardClicked(card);
                });
            }
        }

        public void OnCardClicked(Card card)
        {
            if (selectedCards.Contains(card))
            {
                return;
            }

            if (selectedCards.Count == limit)
            {
                selectedCards.RemoveAt(0);
            }

            selectedCards.Add(card);
        }

        public void DoneClick()
        {
            if (selectedCards.Count < minCount)
            {
                return;
            }

            var message = new CardListMessage(selectedCards).ToNetworkMessage(NetworkManager.Instance.Me.Id);
            message.ResponseTo = NetworkManager.Instance.RespondingTo;
            NetworkManager.Instance.Me.Send(message);

            selectedCards.Clear();
            gameObject.SetActive(false);
        }
    }
}
