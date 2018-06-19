using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkPersian : IPokemonCard
    {
        public DarkPersian(Player owner) : base(owner)
        {
            PokemonName = "Dark Persian";
			EvolvesFrom = "Meowth"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 0;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
				new Fascinate(),
				new PoisonClaws()
            };
			
        }
    }
}
