using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using System;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class FullHealEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;

        [DynamicInput("Targeting type", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Full Heal";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            attachedTo.IsAsleep = false;
            attachedTo.IsConfused = false;
            attachedTo.IsParalyzed = false;
            attachedTo.IsPoisoned = false;
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            PokemonCard target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent);

            FullHealPokemon(target);
        }

        private void FullHealPokemon(PokemonCard pokemon)
        {
            pokemon.IsAsleep = false;
            pokemon.IsConfused = false;
            pokemon.IsParalyzed = false;
            pokemon.IsPoisoned = false;
        }
    }
}
