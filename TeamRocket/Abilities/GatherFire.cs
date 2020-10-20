using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class GatherFire : Ability
    {
        public GatherFire(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
            Name = "Gather Fire";
            Description = "Move one Fire energy from another pokemon and attach it to Charmander";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            var allPokemons = new List<PokemonCard>(owner.BenchedPokemon);
            allPokemons.Add(owner.ActivePokemonCard);
            allPokemons.Remove(PokemonOwner);

            var availablePokemons = allPokemons.Where(card => card.AttachedEnergy.Any(energy => energy.EnergyType == EnergyTypes.Fire));

            PokemonCard selectedPokemon = null;
            EnergyCard selectedEnergyCard = null;

            while(selectedEnergyCard == null)
            {
                var message = new SelectFromYourPokemonMessage("Select pokemon to move energy from").ToNetworkMessage(owner.Id);

                var selectedPokemonId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

                selectedPokemon = allPokemons.First(x => x.Id.Equals(selectedPokemonId));

                selectedEnergyCard = selectedPokemon.AttachedEnergy.FirstOrDefault(e => e.EnergyType == EnergyTypes.Fire);
            }

            selectedPokemon.AttachedEnergy.Remove(selectedEnergyCard);
            PokemonOwner.AttachedEnergy.Add(selectedEnergyCard);
        }
    }
}
