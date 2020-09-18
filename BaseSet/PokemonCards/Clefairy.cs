using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Clefairy : PokemonCard
    {
        public Clefairy(Player owner) : base(owner)
        {
            PokemonName = "Clefairy";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Sing(),
				new Metronome()
            };
			
        }
    }
}
