using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Ekans : PokemonCard
    {
        public Ekans(Player owner) : base(owner)
        {
            PokemonName = "Ekans";
			Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Bite10(),
				new PoisonSting()
            };
			
        }
    }
}
