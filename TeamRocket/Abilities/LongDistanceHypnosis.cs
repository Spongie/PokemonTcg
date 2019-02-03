using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class LongDistanceHypnosis : Ability
    {
        public LongDistanceHypnosis(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
            Name = "Long Distance Hypnosis";
            Description = "Flip a coin. If heads, the defending pokemon is now asleep; if tails, your active pokemon is now asleep";
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
