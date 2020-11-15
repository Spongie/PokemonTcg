using CardEditor.Views;
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
            TriggerType = TriggerType.TakesDamage;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            var damageToPrevent = Modifer < 1 ? Math.Ceiling(damageTaken * Modifer) : Modifer;

            if (damageToPrevent % 5 == 0 && damageToPrevent % 10 != 0)
            {
                if (RoundDown)
                {
                    damageToPrevent -= 5;
                }
                else
                {
                    damageToPrevent += 5;
                }
            }

            owner.ActivePokemonCard.DamageCounters -= (int)damageToPrevent;

            if (owner.ActivePokemonCard.DamageCounters < 0)
            {
                owner.ActivePokemonCard.DamageCounters = 0;
            }
        }
    }
}
