using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class TakesDamagesOnAttacked : Ability
    {
        private int damageReturned;

        public TakesDamagesOnAttacked() :this(null)
        {

        }

        public TakesDamagesOnAttacked(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.TakesDamage;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            opponent.ActivePokemonCard.DealDamage(DamageReturned, game, PokemonOwner, false, false);
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
