using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SwapDamageEffect : DataModel, IEffect
    {
        private TargetingMode firstTarget;
        private TargetingMode secondTarget;

        public string EffectType
        {
            get
            {
                return "Swap damage";
            }
        }

        [DynamicInput("First Target?", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode FirstTarget
        {
            get { return firstTarget; }
            set
            {
                firstTarget = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Second Target?", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode SecondTarget
        {
            get { return secondTarget; }
            set
            {
                secondTarget = value;
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
            var first = Targeting.AskForTargetFromTargetingMode(FirstTarget, game, caster, opponent, pokemonSource);
            var second = Targeting.AskForTargetFromTargetingMode(SecondTarget, game, caster, opponent, pokemonSource);

            if (first == null || second == null)
            {
                return;
            }

            int firstDamage = first.DamageCounters;
            int secondDamage = second.DamageCounters;

            first.DamageCounters = secondDamage;
            second.DamageCounters = firstDamage;

            game.SendEventToPlayers(new DamageTakenEvent()
            {
                Damage = firstDamage,
                PokemonId = second.Id,
                DamageType = pokemonSource.Type
            });

            game.SendEventToPlayers(new DamageTakenEvent()
            {
                Damage = secondDamage,
                PokemonId = first.Id,
                DamageType = pokemonSource.Type
            });
        }
    }
}
