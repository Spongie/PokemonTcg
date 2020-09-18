using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Beedrill : PokemonCard
    {
        public Beedrill(Player owner) : base(owner)
        {
            PokemonName = "Beedrill";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Kakuna;
            Hp = 80;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 0;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.Fighting;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new Twineedle(),
				new PoisonSting()
            };
			
        }
    }
}
