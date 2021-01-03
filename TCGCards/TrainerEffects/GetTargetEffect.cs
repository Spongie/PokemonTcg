using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class GetTargetEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Get Target";
            }
        }

        private string name;
        private TargetingMode targetingMode;

        [DynamicInput("Target to check", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Name filter")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard, name).Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            string info = string.Empty;

            if (!string.IsNullOrEmpty(Name))
            {
                var owner = Targeting.IsYours(TargetingMode) ? "your" : "your opponents";
                info = $"Select 1 of {owner} Pokémon with {Name} in it's name";
            }

            game.LastTarget = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource, info, Name);
        }
    }
}
