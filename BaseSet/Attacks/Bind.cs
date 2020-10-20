using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Bind : Attack
    {
        public Bind()
        {
            Name = "Bind";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            AttackUtils.FlipCoinIfHeadsParalyzed(game.GameLog, opponent.ActivePokemonCard);
        }
    }
}
