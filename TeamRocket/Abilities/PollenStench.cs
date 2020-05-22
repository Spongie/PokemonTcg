using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class PollenStench : Ability
    {
        public PollenStench(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
            Name = "Pollen Stench";
            Description = "Flip a coin, If heads, the defending pokemon is now confused; if tails, your active pokemon is nor confused";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
                opponent.ActivePokemonCard.IsConfused = true;
            else
                owner.ActivePokemonCard.IsConfused = true;
        }
    }
}
