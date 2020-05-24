using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Doubleslap : Attack
    {
        public Doubleslap()
        {
            Name = "Doubleslap";
            Description = "Flip 2 coins. This attack does 30 damage times number of heads.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var heads = CoinFlipper.FlipCoins(2);

            game.GameLog.AddMessage(owner.NetworkPlayer.Name + $" Flips 2 coins and gets {heads} heads");
            var damage = heads * 30;
            owner.ActivePokemonCard.DamageCounters += damage;

            game.GameLog.AddMessage(owner.ActivePokemonCard.GetName() + $" Takes {damage} damage");
        }
    }
}
