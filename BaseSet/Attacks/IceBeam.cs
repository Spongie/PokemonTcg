using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class IceBeam : Attack
    {
        public IceBeam()
        {
            Name = "Ice Beam";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            AttackUtils.FlipCoinIfHeadsParalyzed(game.GameLog, opponent.ActivePokemonCard);
        }
    }
}
