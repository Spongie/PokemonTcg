﻿using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class ImposterProfessorOak : TrainerCard
    {
        public ImposterProfessorOak()
        {
            Name = "Imposter Professor Oak";
            Description = "Your opponent shuffles his or her hand into their deck, then draws 7 cards";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            foreach (var card in opponent.Hand)
            {
                opponent.Deck.Cards.Push(card);
            }

            opponent.Deck.Shuffle();

            opponent.DrawCards(7);
        }
    }
}
