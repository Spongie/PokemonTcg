using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkingCore;
using NetworkingCore.Messages;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace Server.Services
{
    public class GameService : IService
    {
        public GameField theOnlyActiveGame;

        public void InitTypes()
        {
            //TODO
        }

        public ClientGameField HostGame(NetworkId hostPlayer)
        {
            theOnlyActiveGame = new GameField();

            var p = new Player(MasterServer.Instance.Clients[hostPlayer]);
            p.SetDeck(new Deck
            {
                Cards = new Stack<Card>(new List<Card>
                {
                    new Oddish(p),
                    new Oddish(p),
                    new Oddish(p),
                    new Oddish(p),
                    new DarkGloom(p),
                    new DarkGloom(p),
                    new DarkGloom(p),
                    new DarkGloom(p),
                    new DarkVileplume(p),
                    new DarkVileplume(p),
                    new DarkVileplume(p),
                    new DarkVileplume(p),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy()
                })
            });

            theOnlyActiveGame.Players.Add(p);

            return new ClientGameField(theOnlyActiveGame);
        }

        public ClientGameField JoinTheActiveGame(NetworkId playerToJoin)
        {
            var p = new Player(MasterServer.Instance.Clients[playerToJoin]);
            p.SetDeck(new Deck
            {
                Cards = new Stack<Card>(new List<Card>
                {
                    new Oddish(p),
                    new Oddish(p),
                    new Oddish(p),
                    new Oddish(p),
                    new DarkGloom(p),
                    new DarkGloom(p),
                    new DarkGloom(p),
                    new DarkGloom(p),
                    new DarkVileplume(p),
                    new DarkVileplume(p),
                    new DarkVileplume(p),
                    new DarkVileplume(p),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy(),
                    new GrassEnergy()
                })
            });

            theOnlyActiveGame.Players.Add(p);

            theOnlyActiveGame.StartGame();

            SendUpdateToPlayers(theOnlyActiveGame.Players.Where(player => !player.Id.Equals(p.Id)));

            return new ClientGameField(theOnlyActiveGame);
        }

        public GameField SetActivePokemon(NetworkId playerId, PokemonCard pokemonCard)
        {
            theOnlyActiveGame.OnActivePokemonSelected(playerId, pokemonCard);

            return theOnlyActiveGame;
        }

        //TODO BENCH

        private void SendUpdateToPlayers(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                var x = new GameFieldMessage(theOnlyActiveGame);
                player.NetworkPlayer.Send(x.ToNetworkMessage(MasterServer.Instance.Id));
            }
        }
    }
}
