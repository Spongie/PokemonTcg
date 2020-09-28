using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Poisonpowder : Attack
    {
        public Poisonpowder()
        {
            Name = "Poisonpowder";
            Description = "The Defending Pokémon is now Poisoned.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.IsPoisoned = true;
        }
    }
}
