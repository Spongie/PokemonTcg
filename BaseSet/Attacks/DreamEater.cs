using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class DreamEater : Attack
    {
        public DreamEater()
        {
            Name = "Dream Eater";
            Description = "You can't use this attack unless the Defending Pokémon is Asleep.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return opponent.ActivePokemonCard.IsAsleep && base.CanBeUsed(game, owner, opponent);
        }
    }
}
