using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class AttachEnergyCardFromDiscard : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private EnergyTypes energyType = EnergyTypes.All;
        private bool canUseWithoutTarget;
        private int amount;
        private bool individualTargets;

        public string EffectType
        {
            get
            {
                return "Attach energy from discard";
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

        [DynamicInput("Energytype", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Use without target?", InputControl.Boolean)]
        public bool CanUseWithoutTarget
        {
            get { return canUseWithoutTarget; }
            set
            {
                canUseWithoutTarget = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Individual targets?", InputControl.Boolean)]
        public bool IndividualTargets
        {
            get { return individualTargets; }
            set
            {
                individualTargets = value;
                FirePropertyChanged();
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (CanUseWithoutTarget)
            {
                return true;
            }

            return caster.DiscardPile.OfType<EnergyCard>().Count(x => EnergyType == EnergyTypes.All || x.EnergyType == EnergyType) > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var choices = caster.DiscardPile.OfType<EnergyCard>().Where(x => EnergyType == EnergyTypes.All || x.EnergyType == EnergyType).ToList();

            if (choices.Count == 0)
            {
                return;
            }

            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(choices.OfType<Card>().ToList(), Amount).ToNetworkMessage(game.Id));
            var cards = response.Cards.Select(id => game.Cards[id]);

            if (individualTargets)
            {
                foreach (EnergyCard card in cards)
                {
                    var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource, $"Select Pokémon to attach {card.Name} to");
                    target.AttachEnergy(card, game);
                }
            }
            else
            {
                var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource, "Select Pokémon to attach energy to");

                foreach (var card in cards)
                {
                    target.AttachEnergy((EnergyCard)card, game);
                }
            }
        }
    }
}
