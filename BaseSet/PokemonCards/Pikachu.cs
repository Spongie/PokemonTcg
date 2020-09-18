using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Pikachu : PokemonCard
    {
        public Pikachu(Player owner) : base(owner)
        {
            PokemonName = "Pikachu";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Gnaw(),
				new ThunderJolt()
            };
			
        }
    }
}
