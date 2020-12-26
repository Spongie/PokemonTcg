using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class DamageTakenChangerAbility : Ability, IDamageTakenModifier
    {
        private bool onlyPreventBasic;
        private int newDamage;
        private int minimumDamageBeforePrevent;

        public DamageTakenChangerAbility() :this(null)
        {

        }

        public DamageTakenChangerAbility(PokemonCard owner) :base(owner)
        {
            TriggerType = TriggerType.DamageTakenModifier;
        }

        [DynamicInput("Only prevent from basic Pokémon", InputControl.Boolean)]
        public bool OnlyPreventFromBasic
        {
            get { return onlyPreventBasic; }
            set
            {
                onlyPreventBasic = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("New damage taken")]
        public int NewDamage
        {
            get { return newDamage; }
            set
            {
                newDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only prevent if this or more damage")]
        public int MinimumDamageBeforePrevent
        {
            get { return minimumDamageBeforePrevent; }
            set
            {
                minimumDamageBeforePrevent = value;
                FirePropertyChanged();
            }
        }


        public int GetModifiedDamage(int damageTaken, PokemonCard damageSource, GameField game)
        {
            if (damageTaken < minimumDamageBeforePrevent)
            {
                return damageTaken;
            }

            if (OnlyPreventFromBasic && damageSource != null && damageSource.Stage > 0)
            {
                return damageTaken;
            }

            return newDamage;
        }
    }
}
