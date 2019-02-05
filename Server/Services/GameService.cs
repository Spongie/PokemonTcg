using System.Collections.Generic;
using System.Linq;
using NetworkingCore;
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

        public GameField HostGame(NetworkId hostPlayer)
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

            return theOnlyActiveGame;
        }

        public GameField JoinTheActiveGame(NetworkId playerToJoin)
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

            return theOnlyActiveGame;
        }

        public GameField AddToBench(NetworkId playerId, List<PokemonCard> pokemons)
        {
            foreach (var pokemon in pokemons)
            {
                theOnlyActiveGame.OnBenchPokemonSelected(theOnlyActiveGame.Players.First(p => p.Id.Equals(playerId)), pokemons);
            }

            SendUpdateToPlayers(theOnlyActiveGame.Players.Where(player => !player.Id.Equals(playerId)));

            return theOnlyActiveGame;
        }

        public GameField SetActivePokemon(NetworkId playerId, PokemonCard pokemon)
        {
            theOnlyActiveGame.OnActivePokemonSelected(playerId, pokemon);

            SendUpdateToPlayers(theOnlyActiveGame.Players.Where(player => !player.Id.Equals(playerId)));

            return theOnlyActiveGame;
        }

        public GameField ActivateAbility(Ability ability)
        {
            theOnlyActiveGame.ActivateAbility(ability);
            return theOnlyActiveGame;
        }

        public GameField Attack(Attack attack)
        {
            theOnlyActiveGame.Attack(attack);

            return theOnlyActiveGame;
        }

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
