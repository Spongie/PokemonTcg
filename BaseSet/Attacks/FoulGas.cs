using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class FoulGas : Attack
    {
        public FoulGas()
        {
            Name = "Foul Gas";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Poisoned; if tails, it is now Confused.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin())
            {
                game.GameLog.AddMessage(owner.NetworkPlayer?.Name + $"Flipped a heads {opponent.ActivePokemonCard.GetName()} is now poisoned");
                opponent.ActivePokemonCard.IsPoisoned = true;
            }
            else
            {
                game.GameLog.AddMessage(owner.NetworkPlayer?.Name + $"Flipped a tails {opponent.ActivePokemonCard.GetName()} is now confused");
                opponent.ActivePokemonCard.IsConfused = true;
            }
        }
    }
}
