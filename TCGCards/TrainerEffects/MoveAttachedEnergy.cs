using CardEditor.Views;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class MoveAttachedEnergy : DataModel, IEffect
    {
        private TargetingMode source;
        private TargetingMode target;
        private int amount;
        private bool onlyBasic; 
        private bool fromAnother;
        private bool differentTargetForEachCard;
        private EnergyTypes energyType;

        [DynamicInput("Source", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode Source
        {
            get { return source; }
            set
            {
                source = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode Target
        {
            get { return target; }
            set
            {
                target = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Cards to discard")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only basic", InputControl.Boolean)]
        public bool OnlyBasic
        {
            get { return onlyBasic; }
            set
            {
                onlyBasic = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Different targets for all cards", InputControl.Boolean)]
        public bool DifferentTargetForEachCard
        {
            get { return differentTargetForEachCard; }
            set
            {
                differentTargetForEachCard = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy Types", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("From another", InputControl.Boolean)]
        public bool FromAnother
        {
            get { return fromAnother; }
            set
            {
                fromAnother = value;
                FirePropertyChanged();
            }
        }


        public string EffectType
        {
            get
            {
                return "Move attached energy";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            PokemonCard source;

            while (true)
            {
                source = Targeting.AskForTargetFromTargetingMode(Source, game, caster, opponent, pokemonSource, "Select a pokemon to move energy from");

                if (FromAnother && source == pokemonSource)
                {
                    continue;
                }

                break;
            }
            
            if (source == null)
            {
                return;
            }

            var availableEnergyCards = source.AttachedEnergy
                .Where(energy => !OnlyBasic || (OnlyBasic && energy.IsBasic))
                .Where(energy => EnergyType == EnergyTypes.All || EnergyType == EnergyTypes.None || energy.EnergyType == EnergyType)
                .ToList();

            PokemonCard target;
            bool first = true;
            if (availableEnergyCards.Count <= Amount)
            {
                target = Targeting.AskForTargetFromTargetingMode(Target, game, caster, opponent, pokemonSource, "Select a pokemon to move energy to");
                while (source.AttachedEnergy.Count > 0)
                {
                    var energyCard = source.AttachedEnergy[0];

                    source.AttachedEnergy.RemoveAt(0);

                    game.SendEventToPlayers(new AttachedEnergyDiscardedEvent { DiscardedCard = energyCard, FromPokemonId = source.Id });

                    if (DifferentTargetForEachCard && !first)
                    {
                        target = Targeting.AskForTargetFromTargetingMode(Target, game, caster, opponent, pokemonSource, "Select a pokemon to move energy to");
                    }

                    target.AttachedEnergy.Add(energyCard);

                    game.SendEventToPlayers(new EnergyCardsAttachedEvent { EnergyCard = energyCard, AttachedTo = target });
                    first = false;
                }

                return;
            }

            var message = new PickFromListMessage(availableEnergyCards.OfType<Card>().ToList(), 1, Amount);
            var ids = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id)).Cards;

            var cards = source.AttachedEnergy.Where(x => ids.Contains(x.Id)).ToList();
            target = Targeting.AskForTargetFromTargetingMode(Target, game, caster, opponent, pokemonSource, "Select a pokemon to move energy to");

            foreach (var card in cards)
            {
                if (DifferentTargetForEachCard && !first)
                {
                    target = Targeting.AskForTargetFromTargetingMode(Target, game, caster, opponent, pokemonSource, "Select a pokemon to move energy to");
                }

                source.AttachedEnergy.Remove(card);

                game.SendEventToPlayers(new AttachedEnergyDiscardedEvent { DiscardedCard = card, FromPokemonId = source.Id });

                target.AttachedEnergy.Add(card);

                game.SendEventToPlayers(new EnergyCardsAttachedEvent { EnergyCard = card, AttachedTo = target });
                first = false;
            }
        }
    }
}
