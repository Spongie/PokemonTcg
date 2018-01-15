﻿using TCGCards.Core;

namespace TCGCards
{
    public abstract class TrainerCard : ICard
    {
        public TrainerCard() : base(null)
        {
                
        }

        public TrainerCard(Player owner) : base(owner)
        {
        }

        public abstract void Process(GameField game, Player caster, Player opponent);
    }
}
