using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Dewgong : PokemonCard
    {
        public Dewgong(Player owner) : base(owner)
        {
            PokemonName = "Dewgong";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Seel;
            Hp = 80;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 3;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new AuroraBeam(),
				new IceBeam()
            };
			
        }
    }
}
