using CardEditor.Views;
using Entities;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class AttachEnergyFromHandAbility : Ability
    {
        private EnergyTypes energyType;
        private EnergyTypes pokemonType;

        public AttachEnergyFromHandAbility() :this(null)
        {

        }

        public AttachEnergyFromHandAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        [DynamicInput("Valid pokemon type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes PokemonType
        {
            get { return pokemonType; }
            set
            {
                pokemonType = value;
                FirePropertyChanged();
            }
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


        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            IEnumerable<Card> energyCards;

            if (energyType != EnergyTypes.All)
            {
                energyCards = PokemonOwner.Owner.Hand.OfType<EnergyCard>().Where(card => energyType == card.EnergyType);
            }
            else
            {
                energyCards = PokemonOwner.Owner.Hand.OfType<EnergyCard>();
            }

            var message = new PickFromListMessage(energyCards.ToList(), 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var energyCard = owner.Hand.First(x => x.Id.Equals(response.Cards.First()));

            var selectPokemonMessage = new SelectFromYourPokemonMessage(energyType).ToNetworkMessage(owner.Id);
            response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(selectPokemonMessage);
            var selectedId = response.Cards.First();

            var pokemon = owner.ActivePokemonCard.Id.Equals(selectedId) ? owner.ActivePokemonCard : owner.BenchedPokemon.ValidPokemonCards.First(x => x.Id.Equals(selectedId));

            pokemon.AttachEnergy((EnergyCard)energyCard, game);
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            bool hasEnergyType;

            if (energyType != EnergyTypes.All)
            {
                hasEnergyType = PokemonOwner.Owner.Hand.OfType<EnergyCard>().Any(card => energyType == card.EnergyType);
            }
            else
            {
                hasEnergyType = PokemonOwner.Owner.Hand.OfType<EnergyCard>().Any();
            }

            return hasEnergyType && base.CanActivate(game, caster, opponent);
        }
    }
}
