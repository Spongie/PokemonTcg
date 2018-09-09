using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class HayFever : PassiveAbility
    {
        public HayFever(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.StopTrainerCast;
        }
    }
}
