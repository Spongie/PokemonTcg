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
                return (PokemonCard)game.Cards[response.Cards.First()];
            }

            return null;
        }

        public static void DiscardCardsFromHand(Player player, int amount, bool shuffleIntoDeck = false)
        {
            DiscardCardsFromHand(player, amount, new List<IDeckFilter>(), shuffleIntoDeck);
        }

        public static void DiscardCardsFromHand(Player player, int amount, List<IDeckFilter> filters, bool shuffleIntoDeck = false)
        {
            var message = new DiscardCardsMessage(amount, filters);
            var response = player.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(player.Id));

            if (shuffleIntoDeck)
            {
                var cardsToDiscard = response.Cards.Select(id => player.Hand.First(x => x.Id.Equals(id))).ToList();
                player.Deck.ShuffleInCards(cardsToDiscard);

                foreach (var card in cardsToDiscard)
                {
                    player.Hand.Remove(card);
                }

                player.TriggerDiscardEvent(cardsToDiscard);
            }
            else
            {
                foreach (var id in response.Cards)
                {
                    var card = player.Hand.First(x => x.Id.Equals(id));
                    player.DiscardCard(card);
                }
            }
        }
    }
}
