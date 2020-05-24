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

            var message = new PickFromListMessage(availablePokemons, 1).ToNetworkMessage(owner.Id);

            var selectedEnergyId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

            var selectedEnergy = availablePokemons.SelectMany(pokemon => pokemon.AttachedEnergy).First(x => x.Id.Equals(selectedEnergyId));

            owner.AttachEnergyToPokemon(selectedEnergy, PokemonOwner, null);

            foreach (var pokemon in allPokemons)
            {
                if (pokemon.AttachedEnergy.Contains(selectedEnergy))
                {
                    pokemon.AttachedEnergy.Remove(selectedEnergy);
                    break;
                }
            }
        }
    }
}
