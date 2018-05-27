using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    public class Psyshock : Attack
    {
        public Psyshock()
        {
            Name = "Psyshock";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
            {
                opponent.ActivePokemonCard.IsParalyzed = true;
            }
        }
    }
}
