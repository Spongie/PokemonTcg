using BaseSet.PokemonCards;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace BaseSet.Attacks
{
    internal class Agility : Attack
    {
        private Raichu raichu;

        public Agility(Raichu raichu)
        {
            this.raichu = raichu;
            Name = "Agility";
            Description = "Flip a coin. If heads, during your opponent's next turn, prevent all effects of attacks, including damage, done to Raichu.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
            {
                game.GameLog.AddMessage(owner.NetworkPlayer.Name + " Flips a coin and it's tails, nothing happens");
                return;
            }

            game.GameLog.AddMessage(owner.NetworkPlayer.Name + " Flips a coin and it's heads, effect applied");
            
            game.AttackStoppers.Add(new AttackStopper((defender) =>
            {
                return defender.Id == raichu.Id;
            }));
        }
    }
}
