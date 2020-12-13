using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class TakesDamagesOnAttacked : Ability
    {
        private int damageReturned;
        private bool attackBack;

        public TakesDamagesOnAttacked() :this(null)
        {

        }

        public TakesDamagesOnAttacked(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.TakesDamage;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (attackBack)
            {
                var damage = DamageCalculator.GetDamageAfterWeaknessAndResistance(damageTaken, PokemonOwner, opponent.ActivePokemonCard, null);
                opponent.ActivePokemonCard.DealDamage(damage, game, PokemonOwner, true);
            }
            {
                opponent.ActivePokemonCard.DealDamage(DamageReturned, game, PokemonOwner, false);
            }
        }

        [DynamicInput("Attack back for same damage", InputControl.Boolean)]
        public bool AttackBack
        {
            get { return attackBack; }
            set
            {
                attackBack = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage returned")]
        public int DamageReturned
        {
            get { return damageReturned; }
            set
            {
                damageReturned = value;
                FirePropertyChanged();
            }
        }

    }
}
