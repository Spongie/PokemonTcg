using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public static class GameUtils
    {
        public static PokemonCard SelectOnePokemonCardFromOpponent(GameField game, Player askingPlayer)
        {
            var response = askingPlayer.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectOpponentPokemon(1).ToNetworkMessage(askingPlayer.Id));

            if (response.Cards.Any())
            {
                return (PokemonCard)game.FindCardById(response.Cards.First());
            }

            return null;
        }
    }
}
