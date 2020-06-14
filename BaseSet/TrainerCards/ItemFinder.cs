using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class ItemFinder : TrainerCard
    {
        public ItemFinder(Player owner) : base(owner)
        {
            Name = "Item Finder";
            Description = "Discard 2 other cards from your hand in order to put a Trainer card from your discard pile into your hand";
        }

        public override bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.Hand.Count >= 3 && base.CanCast(game, caster, opponent);
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var message = new PickFromListMessage(caster.Hand.Except(new[] { this }), 2);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(caster.Id));

            var returnResponse = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(caster.DiscardPile.OfType<TrainerCard>(), 1).ToNetworkMessage(game.Id));

            var selectedCard = caster.DiscardPile.First(x => x.Id.Equals(returnResponse.Cards.First()));

            caster.Hand.Add(selectedCard);
            caster.DiscardPile.Remove(selectedCard);

            foreach (var id in response.Cards)
            {
                var card = caster.Hand.First(x => x.Id.Equals(id));
                caster.DiscardCard(card);
            }
        }
    }
}
