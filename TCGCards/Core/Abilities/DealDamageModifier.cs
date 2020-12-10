using CardEditor.Views;
using System;

namespace TCGCards.Core.Abilities
{
    public class DealDamageModifier : Ability, IDamageDealtModifier
    {
        private float modifer;
        private bool roundDown;
        private bool onlyIfAnyDamage;

        [DynamicInput("Only add damage if any damage", InputControl.Boolean)]
        public bool OnlyIfAnyDamage
        {
            get { return onlyIfAnyDamage; }
            set
            {
                onlyIfAnyDamage = value;
                FirePropertyChanged();
            }
        }

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

        public DealDamageModifier() : this(null)
        {

        }

        public DealDamageModifier(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DamageDealtModifier;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {

        }

        public int GetModifiedDamage(int damageDone, GameField game)
        {
            if (damageDone == 0 && OnlyIfAnyDamage)
            {
                return damageDone;
            }

            var damageWithPrevention = Modifer < 1 ? Math.Ceiling(damageDone * Modifer) : damageDone + Modifer;

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
