namespace TCGCards.Core
{
    public abstract class TemporaryAbility : Ability
    {
        protected TemporaryAbility(PokemonCard owner) : this(owner, 2)
        {
        }

        protected TemporaryAbility(PokemonCard owner, int turnCount) :base(owner)
        {
            TurnsLeft = turnCount;
        }

        public int TurnsLeft { get; set; }
    }
}
