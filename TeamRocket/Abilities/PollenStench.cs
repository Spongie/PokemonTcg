using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class PollenStench : Ability
    {
        public PollenStench(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
                opponent.ActivePokemonCard.IsConfused = true;
            else
                owner.ActivePokemonCard.IsConfused = true;
        }
    }
}
