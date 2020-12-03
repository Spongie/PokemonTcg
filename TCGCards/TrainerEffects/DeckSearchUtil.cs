using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public static class DeckSearchUtil
    {
        public static List<Card> SearchDeck(GameField game, Player player, List<IDeckFilter> filters, int amount)
        {
            var message = new DeckSearchMessage(player.Deck, filters, amount).ToNetworkMessage(game.Id);

            if (player.Deck.Cards.Count(card => filters.All(f => f.IsCardValid(card))) == 0)
            {
                player.NetworkPlayer.Send(message);
                return new List<Card>();
            }
            
            var response = player.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;

            return response.Select(x => game.FindCardById(x)).ToList();
        }
    }
}
