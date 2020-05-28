using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class EnergyRemoval : TrainerCard
    {
        public EnergyRemoval()
        {
            Name = "Energy Removal";
            Description = "Choose 1 Energy card attached to 1 of your opponents Pokémon and discard it";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            PokemonCard selectedPokemon;
            do
            {
                var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectOpponentPokemon(1).ToNetworkMessage(game.Id));
                selectedPokemon = (PokemonCard)game.FindCardById(response.Cards.First());
            } while (selectedPokemon.AttachedEnergy.Count == 0);

            if (selectedPokemon.AttachedEnergy.Count == 1)
            {
                selectedPokemon.DiscardEnergyCard(selectedPokemon.AttachedEnergy.First());
                return;
            }

            var energyResponse = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(selectedPokemon.AttachedEnergy, 1).ToNetworkMessage(game.Id));
            selectedPokemon.DiscardEnergyCard(selectedPokemon.AttachedEnergy.First(x => x.Id.Equals(energyResponse.Cards.First())));
        }

        public override bool CanCast(GameField game, Player caster, Player opponent)
        {
            var hasAttachedEnergy = opponent.ActivePokemonCard.AttachedEnergy.Count > 0 || opponent.BenchedPokemon.Any(x => x.AttachedEnergy.Count > 0);
            return base.CanCast(game, caster, opponent);
        }
    }
}
