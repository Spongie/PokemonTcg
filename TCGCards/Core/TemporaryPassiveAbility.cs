namespace TCGCards.Core
{
    public abstract class TemporaryPassiveAbility : PassiveAbility
    {
        protected TemporaryPassiveAbility(PokemonCard owner) : this(owner, 2)
        {
        }

        protected TemporaryPassiveAbility(PokemonCard owner, int turnCount) : base(owner)
        {
            TurnsLeft = turnCount;
        }

        public int TurnsLeft { get; set; }
    }
}
