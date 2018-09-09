using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class LongDistanceHypnosis : Ability
    {
        public LongDistanceHypnosis(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
                opponent.ActivePokemonCard.IsAsleep = true;
            else
                owner.ActivePokemonCard.IsAsleep = true;
        }
    }
}
