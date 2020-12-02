using System.Collections.Generic;
using TCGCards;
using TCGCards.Core.Messages;

namespace Assets.Code.UI.Game.SpecialStateHandlers
{
    public class SelectingOpponentsPokemonHandler : ISpecialStateHandler
    {
        private List<CardRenderer> selectedCards;

        public SelectingOpponentsPokemonHandler(SelectOpponentPokemonMessage message)
        {
            selectedCards = new List<CardRenderer>();
        }

        public void Clicked(CardRenderer cardRenderer)
        {
            if (cardRenderer.card.Owner.Id.Equals(GameController.Instance.myId) || !(cardRenderer.card is PokemonCard))
            {
                return;
            }

            cardRenderer.SetSelected(true);
            selectedCards.Add(cardRenderer);
        }

        public void EnableButtons()
        {
            GameController.Instance.doneButton.SetActive(true);
            GameController.Instance.cancelButton.SetActive(false);
        }
    }
}
