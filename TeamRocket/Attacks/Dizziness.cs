using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Dizziness : Attack
    {
        public Dizziness()
        {
            Name = "Dizziness";
            Description = "Draw a card";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.DrawCards(1);
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }
    }
}
