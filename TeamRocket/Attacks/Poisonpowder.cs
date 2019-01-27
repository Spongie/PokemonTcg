using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PoisonpowderDarkGloom : Attack
    {
        public PoisonpowderDarkGloom()
        {
            Name = "Poisonpowder";
            Description = "The Defending Pokémon is now Poisoned.";
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
            opponent.ActivePokemonCard.IsPoisoned = true;
        }
    }
}
