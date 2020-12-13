using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class DiscardEnergyFromTarget : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.ActivePokemonCard.AttachedEnergy.Any())
            {
                return;
            }
            else if (opponent.ActivePokemonCard.AttachedEnergy.Count == 1)
            {
                var energyCard = opponent.ActivePokemonCard.AttachedEnergy[0];
                opponent.ActivePokemonCard.DiscardEnergyCard(energyCard, game);
                return;
            }

            var message = new PickFromListMessage(opponent.ActivePokemonCard.AttachedEnergy.OfType<Card>().ToList(), 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);
            opponent.ActivePokemonCard.DiscardEnergyCard((EnergyCard)game.Cards[response.Cards.First()], game);

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
