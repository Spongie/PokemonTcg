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

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var allPokemons = new List<PokemonCard>(owner.BenchedPokemon);
            allPokemons.Add(owner.ActivePokemonCard);
            allPokemons.Remove(PokemonOwner);

            var availablePokemons = allPokemons.Where(card => card.AttachedEnergy.Any(energy => energy.EnergyType == EnergyTypes.Fire));

            var message = new PickFromListMessage(availablePokemons, 1).ToNetworkMessage(owner.Id);

            var selectedEnergy = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.OfType<EnergyCard>().First();
            owner.AttachEnergyToPokemon(selectedEnergy, PokemonOwner);

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
