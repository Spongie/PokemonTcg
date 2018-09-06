using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TeamRocket.Attacks
{
    internal class RocketTackle : Attack
    {
        public RocketTackle()
        {
            Name = "Rocket Tackle";
            Description = "Dark Blastoise does 10 damage to itself. Flip a coin. If heads, prevent all damage done to Dark Blastoise during your opponent's next turn. (Any other effects of attacks still happen.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            owner.ActivePokemonCard.DamageCounters += 10;
            
            return 40;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
                return;

            owner.ActivePokemonCard.DamageStoppers.Add(new DamageStopper(() =>
            {
                return true;
            }));
        }
    }
}
