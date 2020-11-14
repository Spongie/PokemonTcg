using CardEditor.Views;
using Entities;
using NetworkingCore;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class MoveEnergyAbility : Ability
    {
        private EnergyTypes energyType;

        public MoveEnergyAbility() :base(null)
        {

        }

        public MoveEnergyAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            
        }

        [DynamicInput("Valid energy type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }


        public override bool CanActivate()
        {
            var pokemons = PokemonOwner.Owner.GetAllPokemonCards().ToList();
            return base.CanActivate() && pokemons.Count >= 2 && pokemons.Any(p => p.AttachedEnergy.Any(e => EnergyType == EnergyTypes.All || e.EnergyType == EnergyType));
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
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

            } while (selectedPokemon == null || selectedPokemon.AttachedEnergy.Count(energy => EnergyType == EnergyTypes.All || EnergyType == energy.EnergyType) == 0);
            
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
                var selectedEnergyId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(selectedPokemon.AttachedEnergy.Where(energy => EnergyType == EnergyTypes.All || EnergyType == energy.EnergyType), 1).ToNetworkMessage(owner.Id));
                var energyCard = selectedPokemon.AttachedEnergy.FirstOrDefault(card => card.Id.Equals(selectedEnergyId.Cards.First()));

                selectedPokemon.AttachedEnergy.Remove(energyCard);
                newPokemon.AttachedEnergy.Add(energyCard);
            }
        }
    }
}
