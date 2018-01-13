using System.Collections.Generic;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class Ram : Attack
    {
        public Ram()
        {
            Name = "Ram";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1),
            };
        }


        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
    }
}
