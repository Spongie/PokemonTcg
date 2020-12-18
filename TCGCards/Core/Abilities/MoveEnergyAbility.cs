using CardEditor.Views;
using Entities;
using NetworkingCore;
using System.Linq;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class MoveEnergyAbility : Ability
    {
        private EnergyTypes energyType;
        private bool attachToSelf;

        public MoveEnergyAbility() : this(null)
        {

        }

        public MoveEnergyAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
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

        [DynamicInput("Attach to self", InputControl.Boolean)]
        public bool AttachToSelf
        {
            get { return attachToSelf; }
            set
            {
                attachToSelf = value;
                FirePropertyChanged();
            }
        }


        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            var pokemons = PokemonOwner.Owner.GetAllPokemonCards().ToList();
            int targetCount = 2;

            if (attachToSelf)
            {
                pokemons.Remove(PokemonOwner);
                targetCount = 1;
            }

            return base.CanActivate(game, caster, opponent) && pokemons.Count >= targetCount && pokemons.Any(p => p.AttachedEnergy.Any(e => EnergyType == EnergyTypes.All || e.EnergyType == EnergyType));
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            PokemonCard sourcePokemon = null;
            do
            {
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select a pokemon to move energy from").ToNetworkMessage(NetworkId.Generate()));
                var selectedId = response.Cards.First();

                sourcePokemon = game.Cards[selectedId] as PokemonCard;

            } while (sourcePokemon == null || sourcePokemon.Id.Equals(PokemonOwner.Id) || sourcePokemon.AttachedEnergy.Count(energy => EnergyType == EnergyTypes.All || EnergyType == energy.EnergyType) == 0);
            
            PokemonCard newPokemon;

            if (attachToSelf)
            {
                newPokemon = PokemonOwner;
            }
            else
            {
                var newPokemonResponse = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select pokemon to receive the energy").ToNetworkMessage(NetworkId.Generate()));
                newPokemon = (PokemonCard)game.Cards[newPokemonResponse.Cards.First()];
            }

            if (sourcePokemon.AttachedEnergy.Count == 1)
            {
                var energyCard = sourcePokemon.AttachedEnergy.First();
                sourcePokemon.AttachedEnergy.Clear();

                game?.SendEventToPlayers(new AttachedEnergyDiscardedEvent
                {
                    FromPokemonId = sourcePokemon.Id,
                    DiscardedCard = energyCard
                });

                newPokemon.AttachEnergy(energyCard, game);
            }
            else
            {
                var selectedEnergyId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(sourcePokemon.AttachedEnergy.Where(energy => EnergyType == EnergyTypes.All || EnergyType == energy.EnergyType).OfType<Card>().ToList(), 1).ToNetworkMessage(owner.Id));
                var energyCard = sourcePokemon.AttachedEnergy.FirstOrDefault(card => card.Id.Equals(selectedEnergyId.Cards.First()));

                sourcePokemon.AttachedEnergy.Remove(energyCard);
                game?.SendEventToPlayers(new AttachedEnergyDiscardedEvent
                {
                    FromPokemonId = sourcePokemon.Id,
                    DiscardedCard = energyCard
                });
                newPokemon.AttachEnergy(energyCard, game);
            }
        }
    }
}
