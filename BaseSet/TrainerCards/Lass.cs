using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class Lass : TrainerCard
    {
        public Lass(Player owner) : base(owner)
        {
            Name = "Lass";
            Description = "You and your opponent show each other your hands, then shuffle all the trainer cards from your hands into your decks";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            game.RevealCardsTo(caster.Hand.Select(x => x.Id).ToList(), opponent);
            game.RevealCardsTo(opponent.Hand.Select(x => x.Id).ToList(), caster);

            DiscardTrainerCards(caster);
            DiscardTrainerCards(opponent);
        }

        private static void DiscardTrainerCards(Player caster)
        {
            var trainerCards = caster.Hand.OfType<TrainerCard>().ToList();

            caster.Deck.ShuffleInCards(trainerCards);

            foreach (var trainerCard in trainerCards)
            {
                caster.Hand.Remove(trainerCard);
            }
        }
    }
}
