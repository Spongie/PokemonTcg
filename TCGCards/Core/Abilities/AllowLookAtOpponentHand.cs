namespace TCGCards.Core.Abilities
{
    public class AllowLookAtOpponentHand : PassiveAbility
    {
        public AllowLookAtOpponentHand() :this(null)
        {

        }

        public AllowLookAtOpponentHand(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.LookAtOpponentHand;
        }
    }
}
