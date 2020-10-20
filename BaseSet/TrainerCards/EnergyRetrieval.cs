using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class EnergyRetrieval : TrainerCard
    {
        public EnergyRetrieval()
        {
            Name = "Energy Retreival";
            Description = "Discard 1 card from your hand. Return up to 2 basic energy cards from your discard pile to your hand";
            Set = Singleton.Get<Set>();
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var availableEnergyCards = caster.DiscardPile.OfType<EnergyCard>().Where(card => card.IsBasic).ToList();
            
            GameUtils.DiscardCardsFromHand(caster, 1, new[] { this });

            if (availableEnergyCards.Count == 0)
            {
                return;
            }

            var message = new PickFromListMessage(availableEnergyCards, 0, 2).ToNetworkMessage(game.Id);

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
