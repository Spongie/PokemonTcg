using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Nidorino : PokemonCard
    {
        public Nidorino(Player owner) : base(owner)
        {
            PokemonName = "Nidorino";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.NidoranMale;
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new DoubleKick(),
				new HornDrill()
            };
			
        }
    }
}
