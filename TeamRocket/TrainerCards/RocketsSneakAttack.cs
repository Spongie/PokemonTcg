using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.TrainerCards
{
    public class RocketsSneakAttack : TrainerCard
    {
        public override string GetName()
        {
            return "Rocket's Sneak Attack";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var message = new PickFromListMessage(opponent.Hand, new List<IDeckFilter> { new DeckFilter() }, 1);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<DeckSearchedMessage>(message.ToNetworkMessage(caster.Id));

            var pickedCard = response.SelectedCards.FirstOrDefault();

            if (pickedCard != null)
            {
                opponent.Hand = opponent.Hand.Except(response.SelectedCards).ToList();
                opponent.Deck.Cards.Push(pickedCard);
                opponent.Deck.Shuffle();
            }
        }

        private class DeckFilter : IDeckFilter
        {
            public bool IsCardValid(ICard card)
            {
                return card is TrainerCard;
            }
        }
    }
}
