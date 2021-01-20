using System;
using System.Linq;
using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class AttachEnergyFromHand : DataModel, IEffect
    {
        private EnergyTypes pokemonType;
        private EnergyTypes energyType;
        private int amount = 1;
        private TargetingMode targetingMode;

        public string EffectType
        {
            get
            {
                return "Attach energy from hand";
            }
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

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var filters = CardUtil.GetCardFilters(CardType.Energy, EnergyType);

            var energyTypeText = EnergyType == EnergyTypes.All ? "Energy" : Enum.GetName(typeof(EnergyTypes), EnergyType) + "Energy";
            var message = new DiscardCardsMessage(Amount, filters)
            {
                MinCount = 0,
                Info = $"Pick up to 2 {energyTypeText} from your hand"
            }.ToNetworkMessage(game.Id);

            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            if (!response.Cards.Any())
            {
                return;
            }

            var energyCard = game.Cards[response.Cards.First()];

            var pokemon = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            pokemon.AttachEnergy((EnergyCard)energyCard, game);
        }
    }
}
