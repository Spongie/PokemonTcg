using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class GatherFire : Ability
    {
        public GatherFire(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var availablePokemons = new List<PokemonCard>(owner.BenchedPokemon);
            availablePokemons.Add(owner.ActivePokemonCard);
            availablePokemons.Remove(PokemonOwner);

            var message = new SelectEnergyFromYourPokemonMessage
            {
                Amount = 1,
                Filter = new EnergyTypeFilter(EnergyTypes.Fire),
                Pokemons = availablePokemons
            }.ToNetworkMessage(owner.Id);

            var selectedEnergy = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.OfType<EnergyCard>().First();
            PokemonOwner.AttachedEnergy.Add(selectedEnergy);

            foreach (var pokemon in availablePokemons)
            {
                if (pokemon.AttachedEnergy.Contains(selectedEnergy))
                {
                    pokemon.AttachedEnergy.Remove(selectedEnergy);
                    break;
                }
            }
        }

        public override void SetTarget(Card target)
        {
            
        }
    }
}
