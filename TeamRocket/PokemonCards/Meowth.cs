using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Meowth : IPokemonCard
    {
        public Meowth(Player owner) : base(owner)
        {
            PokemonName = "Meowth";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
				new CoinHurl()
            };
			
        }
    }
}
