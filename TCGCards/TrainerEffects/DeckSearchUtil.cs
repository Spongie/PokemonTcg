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
            game?.GetOpponentOf(player)?.NetworkPlayer?.Send(new InfoMessage("Opponent is searching their deck...").ToNetworkMessage(game.Id));
            var message = new DeckSearchMessage(player.Deck.Cards.ToList(), filters, amount).ToNetworkMessage(game.Id);
            
            var response = player.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;

            return response.Select(x => game.Cards[x]).ToList();
        }
    }
}
