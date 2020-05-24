using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class NidoranMale : PokemonCard
    {
        public NidoranMale(Player owner) : base(owner)
        {
            PokemonName = "Nidoran♂";
            Hp = 40;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new HornHazard()
            };
			
        }
    }
}
