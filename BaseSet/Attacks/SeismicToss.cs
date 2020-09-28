using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class SeismicToss : Attack
    {
        public SeismicToss()
        {
            Name = "Seismic Toss";
            Description = "";
			DamageText = "60";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 3),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 60;
        }
		
    }
}
