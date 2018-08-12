using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkArbok : IPokemonCard
    {
        public DarkArbok(Player owner) : base(owner)
        {
            PokemonName = "Dark Arbok";
			EvolvesFrom = "Ekans";
            Stage = 0;
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Stare(),
				new PoisonVapor()
            };
			
        }
    }
}