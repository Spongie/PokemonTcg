using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Magmar : PokemonCard
    {
        public Magmar(Player owner) : base(owner)
        {
            PokemonName = "Magmar";
            Set = Singleton.Get<Set>();
            Hp = 50;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 2;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new FirePunch(),
				new Flamethrower()
            };
			
        }
    }
}
