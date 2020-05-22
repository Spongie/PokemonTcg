using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class Sinkhole : Ability
    {
        public Sinkhole(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.OpponentRetreats;
            Name = "Sinkhole";
            Description = "Whenever your oppoent's active pokemon retreats your opponent flips a coin. If tails this power does 20 damage to that pokemon";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.TAILS)
            {
                opponent.ActivePokemonCard.DealDamage(new Damage(0, 20), log);
            }
        }
    }
}
