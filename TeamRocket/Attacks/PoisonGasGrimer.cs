using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PoisonGasGrimer : Attack
    {
        public PoisonGasGrimer()
        {
            Name = "Poison Gas";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Poisoned.";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.IsAsleep = true;
        }
    }
}
