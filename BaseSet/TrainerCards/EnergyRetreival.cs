using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class EnergyRetreival : TrainerCard
    {
        public EnergyRetreival()
        {
            Name = "Energy Retreival";
            Description = "Discard 1 card from your hand. Return up to 2 basic energy cards from your discard pile to your hand";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            GameUtils.DiscardCardsFromHand(caster, 1);

            var message = new PickFromListMessage(caster.DiscardPile.OfType<EnergyCard>().Where(card => card.IsBasic), 0, 2).ToNetworkMessage(game.Id);

            foreach (var cardId in caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards)
            {
                var card = caster.DiscardPile.First(x => x.Id.Equals(cardId));
                caster.Hand.Add(card);
                caster.DiscardPile.Remove(card);
            }
        }

        public override bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.Hand.Count >= 2 && base.CanCast(game, caster, opponent);
        }
    }
}
