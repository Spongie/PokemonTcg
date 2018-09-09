using System;
using System.Collections.Generic;
using System.Text;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.TrainerCards
{
    public class ImposterOaksRevenge : TrainerCard
    {
        public override string GetName()
        {
            return "Discard a card from your hand in order to play this card. Your opponent shuffles his or her hand into his or her deck, then draws 4 cards.";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new DiscardCardsMessage(1).ToNetworkMessage(caster.Id));
            caster.DiscardCards(response.Cards);

            opponent.Deck.ShuffleInCards(opponent.Hand);
            opponent.Hand.Clear();
            opponent.DrawCards(4);
        }

        public override bool CanCast(GameField game, Player caster, Player opponent) => caster.Hand.Count >= 2;
    }
}
