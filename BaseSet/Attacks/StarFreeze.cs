using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class StarFreeze : Attack
    {
        public StarFreeze()
        {
            Name = "Star Freeze";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
				new Energy(EnergyTypes.Colorless, 2)
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
