using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class PoisonSting : Attack
    {
        public PoisonSting()
        {
            Name = "Poison Sting";
            Description = "Flip a coin. If heads, Defending Pokémon is now Poisoned.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            AttackUtils.FlipCoinIfHeadsPoisoned(game.GameLog, opponent.ActivePokemonCard);
        }
    }
}
