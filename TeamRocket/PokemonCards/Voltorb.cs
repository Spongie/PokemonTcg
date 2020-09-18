using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Voltorb : PokemonCard
    {
        public Voltorb(Player owner) : base(owner)
        {
            Hp = 40;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Electric;
            Weakness = EnergyTypes.Fighting;
            Resistance = EnergyTypes.None;
            Stage = 0;
            RetreatCost = 1;
            PokemonName = PokemonNames.Voltorb;
            Attacks = new List<Attack>
            {
                new SpeedBall()
            };
        }
    }
}
