using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Ponyta : PokemonCard
    {
        public Ponyta(Player owner) : base(owner)
        {
            PokemonName = "Ponyta";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new SmashKick(),
				new FlameTail()
            };
			
        }
    }
}
