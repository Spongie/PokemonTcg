using System.Collections.Generic;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class Bite : Attack
    {
        public Bite()
        {
            Name = "Bite";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1),
                new Energy(EnergyTypes.Grass, 1)
            };
        }


        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
    }
}
