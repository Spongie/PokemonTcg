using System.Collections.Generic;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class IceBeam : Attack
    {
        public IceBeam()
        {
            Name = "IceBeam";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 3)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if(CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
                opponent.ActivePokemonCard.IsParalyzed = true;
        }
    }
}
