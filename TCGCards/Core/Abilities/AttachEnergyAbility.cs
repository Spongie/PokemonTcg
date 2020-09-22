using Entities;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class AttachEnergyAbility : Ability
    {
        private EnergyTypes[] energyTypes;

        public AttachEnergyAbility(PokemonCard pokemonOwner, EnergyTypes[] energyTypes, EnergyTypes[] targetTypes, int usageLimit) : base(pokemonOwner)
        {
            Usages = usageLimit;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            IEnumerable<EnergyCard> energyCards;

            if (energyTypes.Any())
            {
                energyCards = PokemonOwner.Owner.Hand.OfType<EnergyCard>().Where(card => energyTypes.Contains(card.EnergyType));
            }
            else
            {
                energyCards = PokemonOwner.Owner.Hand.OfType<EnergyCard>();
            }

            var message = new PickFromListMessage(energyCards, 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var energyCard = owner.Hand.First(x => x.Id.Equals(response.Cards.First()));

            var selectPokemonMessage = new SelectFromYourPokemonMessage(energyTypes).ToNetworkMessage(owner.Id);
            response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(selectPokemonMessage);
            var selectedId = response.Cards.First();

            var pokemon = owner.ActivePokemonCard.Id.Equals(selectedId) ? owner.ActivePokemonCard : owner.BenchedPokemon.First(x => x.Id.Equals(selectedId));

            pokemon.AttachedEnergy.Add((EnergyCard)energyCard);
            owner.Hand.Remove(energyCard);
        }

        public override bool CanActivate()
        {
            bool hasEnergyType;

            if (energyTypes.Any())
            {
                hasEnergyType = PokemonOwner.Owner.Hand.OfType<EnergyCard>().Any(card => energyTypes.Contains(card.EnergyType));
            }
            else
            {
                hasEnergyType = PokemonOwner.Owner.Hand.OfType<EnergyCard>().Any();
            }

            return base.CanActivate();
        }
    }
}
