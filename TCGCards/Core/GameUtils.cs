using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public static class GameUtils
    {
        public static PokemonCard SelectOnePokemonCardFromOpponent(GameField game, Player askingPlayer)
        {
            var response = askingPlayer.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectOpponentPokemonMessage(1).ToNetworkMessage(askingPlayer.Id));

            if (response.Cards.Any())
            {
                return (PokemonCard)game.FindCardById(response.Cards.First());
            }

            return null;
        }

        public static void DiscardCardsFromHand(Player player, int amount)
        {
            DiscardCardsFromHand(player, amount, new IDeckFilter[] { });
        }

        public static void DiscardCardsFromHand(Player player, int amount, IEnumerable<IDeckFilter> filters)
        {
            var message = new DiscardCardsMessage(amount, filters);
            var response = player.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(player.Id));

            foreach (var id in response.Cards)
            {
                var card = player.Hand.First(x => x.Id.Equals(id));
                player.DiscardCard(card);
            }
        }
    }
}
