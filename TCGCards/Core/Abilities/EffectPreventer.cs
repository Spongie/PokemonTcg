namespace TCGCards.Core.Abilities
{
    public class EffectPreventer : PassiveAbility
    {
        public EffectPreventer() : this(null)
        {

        }
        public EffectPreventer(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
        }
    }
}