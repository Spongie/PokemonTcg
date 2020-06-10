namespace TCGCards.Core.Abilities
{
    public class TakesDamagesOnAttacked : Ability
    {
        private readonly int damageReturned;

        public TakesDamagesOnAttacked(PokemonCard pokemonOwner, int damageReturned) : base(pokemonOwner)
        {
            TriggerType = TriggerType.TakesDamage;
            this.damageReturned = damageReturned;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            opponent.ActivePokemonCard.DamageCounters += damageReturned;
        }
    }
}
