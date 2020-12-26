using CardEditor.Views;
using System;

namespace TCGCards.Core.Abilities
{
    public class PreventDamageAbility : PassiveAbility, IDamageTakenModifier
    {
        private int minimumBeforePrevention;
        private int damageToPrevent;

        public PreventDamageAbility() :this(null)
        {

        }

        public PreventDamageAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DamageTakenModifier;
        }

        [DynamicInput("How much to prevent")]
        public int DamageToPrevent
        {
            get { return damageToPrevent; }
            set
            {
                damageToPrevent = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only prevent if damage is >= this")]
        public int MinimumBeforePrevention
        {
            get { return minimumBeforePrevention; }
            set
            {
                minimumBeforePrevention = value;
                FirePropertyChanged();
            }
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (damageTaken < minimumBeforePrevention)
            {
                return;
            }

            if (damageTaken < DamageToPrevent)
            {
                PokemonOwner.DamageCounters -= damageTaken;
            }
            else
            {
                PokemonOwner.DamageCounters -= DamageToPrevent;
            }
        }

        public int GetModifiedDamage(int damageTaken, PokemonCard damageSource, GameField game)
        {
            if (damageTaken < minimumBeforePrevention)
            {
                return damageTaken;
            }

            return (int)Math.Max(damageTaken - damageToPrevent, 0);
        }
    }
}
