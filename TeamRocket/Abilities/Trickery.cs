using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class Trickery : Ability
    {
        public Trickery(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var message = new PickFromListMessage(owner.PrizeCards, new AnyCardFilter(), 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<DeckSearchedMessage>(message);

            var topCard = owner.Deck.DrawCard();
            var selectedCard = response.SelectedCards.First();

            var selectedIndex = owner.PrizeCards.IndexOf(selectedCard);
            owner.PrizeCards.RemoveAt(selectedIndex);
            owner.PrizeCards.Insert(selectedIndex, topCard);
            owner.Deck.Cards.Push(selectedCard);
        }

        public override void SetTarget(Card target)
        {
            Target = target;
        }

        public Card Target { get; private set; }
    }
}
