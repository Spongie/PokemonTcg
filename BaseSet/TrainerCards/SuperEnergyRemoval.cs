using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class SuperEnergyRemoval : TrainerCard
    {
        public SuperEnergyRemoval()
        {
            Name = "Super Energy Removal";
            Description = "Discard 1 Energy card attached to 1 of your own pokemon. Then discard up to 2 Energy cards from an opponents pokemon";
            Set = Singleton.Get<Set>();
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            PokemonCard selectedPokemon;

            do
            {
                var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage().ToNetworkMessage(game.Id));
                selectedPokemon = (PokemonCard)game.FindCardById(response.Cards.First());
            } while (selectedPokemon.AttachedEnergy.Count == 0);

            if (selectedPokemon.AttachedEnergy.Count == 1)
            {
                selectedPokemon.DiscardEnergyCard(selectedPokemon.AttachedEnergy.First());
            }
            else
            {
                var availableChoices = selectedPokemon.AttachedEnergy;
                var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(availableChoices, 1).ToNetworkMessage(game.Id)).Cards;

                selectedPokemon.AttachedEnergy.Remove(selectedPokemon.AttachedEnergy.First(x => x.Id.Equals(response.First())));
            }

            var energyResponse = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(selectedPokemon.AttachedEnergy, 0, 2).ToNetworkMessage(game.Id));
            selectedPokemon.DiscardEnergyCard(selectedPokemon.AttachedEnergy.First(x => x.Id.Equals(energyResponse.Cards.First())));
        }

        public override bool CanCast(GameField game, Player caster, Player opponent)
        {
            var hasAttachedEnergy = caster.ActivePokemonCard.AttachedEnergy.Count > 0 || caster.BenchedPokemon.Any(x => x.AttachedEnergy.Count > 0);
            return base.CanCast(game, caster, opponent);
        }
    }
}
