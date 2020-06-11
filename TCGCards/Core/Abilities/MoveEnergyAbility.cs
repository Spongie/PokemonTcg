using NetworkingCore;
using System;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class MoveEnergyAbility : Ability
    {
        private readonly EnergyTypes[] availableTypes;

        public MoveEnergyAbility(PokemonCard pokemonOwner, string abilityName, params EnergyTypes[] availableTypes) : base(pokemonOwner)
        {
            this.availableTypes = availableTypes;
            Name = abilityName;
            string types = string.Join(" or ", availableTypes.Select(type => Enum.GetName(typeof(EnergyTypes), type)));
            Description = $"As often as you like during your turn (before your attack), you may take 1 {types} card attached to 1 of your Pokémon and attach it to a different one. " +
                $"This power can't be used if Venusaur is Asleep, Confused, or Paralyzed.";
        }

        public override bool CanActivate()
        {
            var pokemons = PokemonOwner.Owner.GetAllPokemonCards().ToList();
            return base.CanActivate() && pokemons.Count >= 2 && pokemons.Any(p => p.AttachedEnergy.Any(e => availableTypes.Contains(e.EnergyType)));
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            PokemonCard selectedPokemon = null;
            do
            {
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select a pokemon to move energy from").ToNetworkMessage(NetworkId.Generate()));
                var selectedId = response.Cards.First();

                foreach (var pokemon in owner.GetAllPokemonCards())
                {
                    if (pokemon.Id.Equals(selectedId))
                    {
                        selectedPokemon = pokemon;
                        break;
                    }
                }

            } while (selectedPokemon == null || selectedPokemon.AttachedEnergy.Count(energy => availableTypes.Contains(energy.EnergyType)) == 0);
            
            var newPokemonId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select pokemon to receive the energy").ToNetworkMessage(NetworkId.Generate()));
            PokemonCard newPokemon = null;

            if (owner.ActivePokemonCard.Id.Equals(newPokemonId))
            {
                newPokemon = owner.ActivePokemonCard;
            }
            else
            {
                foreach (var pokemon in owner.BenchedPokemon)
                {
                    if (pokemon.Id.Equals(newPokemonId))
                    {
                        newPokemon = pokemon;
                        break;
                    }
                }
            }

            if (selectedPokemon.AttachedEnergy.Count == 1)
            {
                newPokemon.AttachedEnergy.Add(selectedPokemon.AttachedEnergy.First());
                selectedPokemon.AttachedEnergy.Clear();
            }
            else
            {
                var selectedEnergyId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(selectedPokemon.AttachedEnergy.Where(energy => availableTypes.Contains(energy.EnergyType)), 1).ToNetworkMessage(owner.Id));
                var energyCard = selectedPokemon.AttachedEnergy.FirstOrDefault(card => card.Id.Equals(selectedEnergyId.Cards.First()));

                selectedPokemon.AttachedEnergy.Remove(energyCard);
                newPokemon.AttachedEnergy.Add(energyCard);
            }
        }
    }
}
