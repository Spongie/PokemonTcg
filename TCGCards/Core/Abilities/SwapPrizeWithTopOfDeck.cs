using NetworkingCore;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class SwapPrizeWithTopOfDeck : Ability
    {
        public SwapPrizeWithTopOfDeck() :this(null)
        {

        }

        public SwapPrizeWithTopOfDeck(PokemonCard owner) :base(owner)
        {

        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            var message = new SelectPriceCardsMessage(1).ToNetworkMessage(NetworkId.Generate());
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var selectedCard = game.FindCardById(response.Cards.First());

            owner.PrizeCards.Remove(selectedCard);

            var topOfDeck = owner.Deck.Cards.Pop();

            owner.PrizeCards.Add(topOfDeck);

            owner.Deck.Cards.Push(selectedCard);
        }
    }
}
