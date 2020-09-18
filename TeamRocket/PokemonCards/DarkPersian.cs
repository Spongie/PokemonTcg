using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkPersian : PokemonCard
    {
        public DarkPersian(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkPersian;
			EvolvesFrom = PokemonNames.Meowth;
            Stage = 1;
            Set = Singleton.Get<Set>();
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
