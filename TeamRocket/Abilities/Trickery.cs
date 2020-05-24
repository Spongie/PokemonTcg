using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class Trickery : Ability
    {
        public Trickery(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
            Name = "Trickery";
            Description = "Switch on of your prizes with the top card of your deck";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            var message = new PickFromListMessage(owner.PrizeCards, 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var topCard = owner.Deck.DrawCard();
            var selectedCard = owner.PrizeCards.First(x => x.Id.Equals(response.Cards.First()));

            var selectedIndex = owner.PrizeCards.IndexOf(selectedCard);
            owner.PrizeCards.RemoveAt(selectedIndex);
            owner.PrizeCards.Insert(selectedIndex, topCard);
            owner.Deck.Cards.Push(selectedCard);
        }

        public Card Target { get; private set; }
    }
}
