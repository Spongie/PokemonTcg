using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.PokemonCards.TeamRocket.Attacks;

namespace TCGCards.PokemonCards.TeamRocket
{
    public class Voltorb : IPokemonCard
    {
        public Voltorb(Player owner) : base(owner)
        {
            Hp = 40;
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

        public override string GetName()
        {
            return PokemonNames.Voltorb;
        }
    }
}
