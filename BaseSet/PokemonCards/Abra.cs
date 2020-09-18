using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Abra : PokemonCard
    {
        public Abra(Player owner) : base(owner)
        {
            PokemonName = "Abra";
            Set = Singleton.Get<Set>();
            Hp = 30;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 0;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Psyshock()
            };
			
        }
    }
}
