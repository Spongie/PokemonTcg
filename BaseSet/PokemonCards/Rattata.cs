using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Rattata : PokemonCard
    {
        public Rattata(Player owner) : base(owner)
        {
            PokemonName = "Rattata";
            Set = Singleton.Get<Set>();
            Hp = 30;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 0;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Bite()
            };
			
        }
    }
}
