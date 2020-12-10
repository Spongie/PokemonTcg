using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class AttachmentEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Create attachment";
            }
        }

        private Ability ability;
        private TargetingMode targeting;

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targeting; }
            set
            {
                targeting = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Set Ability", InputControl.Ability)]
        public Ability Ability
        {
            get { return ability; }
            set
            {
                ability = value;
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
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            var ability = Ability.Clone();
            ability.Source = game.CurrentTrainerCard;
            ability.PokemonOwner = target;
            target.TemporaryAbilities.Add(ability);
        }
    }
}
