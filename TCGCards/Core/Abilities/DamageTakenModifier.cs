﻿using CardEditor.Views;
using System;

namespace TCGCards.Core.Abilities
{
    public class DamageTakenModifier : Ability
    {
        private float modifer;
        private bool roundDown;

        [DynamicInput("Round down?", InputControl.Boolean)]
        public bool RoundDown
        {
            get { return roundDown; }
            set
            {
                roundDown = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage modifier (0-1 for %)")]
        public float Modifer
        {
            get { return modifer; }
            set
            {
                modifer = value;
                FirePropertyChanged();
            }
        }

        public DamageTakenModifier() :this(null)
        {

        }

        public DamageTakenModifier(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DamageTakenModifier;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }

        public int GetModifiedDamage(int damageTaken)
        {
            var damageWithPrevention = Modifer < 1 ? Math.Ceiling(damageTaken * Modifer) : damageTaken - Modifer;

            if (damageWithPrevention % 5 == 0 && damageWithPrevention % 10 != 0)
            {
                if (RoundDown)
                {
                    damageWithPrevention -= 5;
                }
                else
                {
                    damageWithPrevention += 5;
                }
            }

            return (int)Math.Max(damageWithPrevention, 0);
        }
    }
}
