using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class MoveDamageFromAllYoursToTarget : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private int damageToMove;

        public string EffectType
        {
            get
            {
                return "Move damage to defender";
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

        public int DamageToMove
        {
            get { return damageToMove; }
            set
            {
                damageToMove = value;
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
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource, "Select Pokémon to move damage to");

            foreach (var pokemon in caster.GetAllPokemonCards())
            {
                if (pokemon.DamageCounters > 0)
                {
                    int damage = Math.Min(pokemon.DamageCounters, DamageToMove);
                    pokemon.Heal(damage, game);
                    target.DealDamage(damage, game, pokemon, false);
                }
            }
        }
    }
}
