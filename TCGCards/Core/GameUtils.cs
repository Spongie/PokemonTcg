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

        public static void DiscardCardsFromHand(Player player, GameField game, DiscardCardSettings discardSettings)
        {
            var message = new DiscardCardsMessage
            {
                MinCount = discardSettings.MinAmount,
                Count = discardSettings.MaxAmount,
                Filters = discardSettings.Filters.ToList(),
            };

            var response = player.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(player.Id));

            if (discardSettings.ShuffleIntoDeck)
            {
                var cardsToDiscard = response.Cards.Select(id => player.Hand.First(x => x.Id.Equals(id))).ToList();
                player.Deck.ShuffleInCards(cardsToDiscard);
                game.LastDiscard = cardsToDiscard.Count;

                foreach (var card in cardsToDiscard)
                {
                    player.Hand.Remove(card);
                }

                player.TriggerDiscardEvent(cardsToDiscard);
            }
            else
            {
                game.LastDiscard = response.Cards.Count;
                foreach (var id in response.Cards)
                {
                    var card = player.Hand.First(x => x.Id.Equals(id));
                    player.DiscardCard(card);
                }
            }
        }
    }
}
