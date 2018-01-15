using System.Collections.Generic;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class SpeedBall : Attack
    {
        public SpeedBall()
        {
            Name = "Speed ball";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
    }
}
