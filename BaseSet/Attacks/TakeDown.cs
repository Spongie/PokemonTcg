using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class TakeDown : Attack
    {
        public TakeDown()
        {
            Name = "Take Down";
            Description = "Arcanine does 30 damage to itself.";
			DamageText = "80";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 80;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.ActivePokemonCard.DamageCounters += 30;
        }
    }
}
