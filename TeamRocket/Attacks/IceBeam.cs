using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class IceBeam : Attack
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

        public override Damage GetDamage(Player owner, Player opponent)
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
