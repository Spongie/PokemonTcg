using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetworkingCore;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace Server.Services
{
    public class GameService : IService
    {
        public ConcurrentDictionary<NetworkId, GameField> ActiveGames { get; private set; }

        public void InitTypes()
        {
            ActiveGames = new ConcurrentDictionary<NetworkId, GameField>();
        }

        public List<GameInfo> GetAvailableGames()
        {
            RemoveCompletedGames();
            var allGames = ActiveGames.Values;

            return allGames.Where(game => game.Players.Count < 2).Select(game => new GameInfo
            {
                HostingPlayer = game.Players.First().NetworkPlayer.Name,
                FormatName = "Unlimited",
                Id = game.Id
            }).ToList();
        }

        public GameField HostGame(NetworkId hostPlayer, List<TypeInfo> deckInfo)
        {
            RemoveCompletedGames();
            var player = new Player(MasterServer.Instance.Clients[hostPlayer]);
            var game = new GameField();
            
            foreach (var type in deckInfo)
            {
                var card = Card.CreateFromTypeInfo(type);
                player.Deck.Cards.Push(card);
            }

            game.Players.Add(player);
            ActiveGames.TryAdd(game.Id, game);

            return game;
        }

        public GameField JoinTheActiveGame(NetworkId playerToJoin, NetworkId gameToJoin, List<TypeInfo> deckInfo)
        {
            RemoveCompletedGames();
            GameField game;

            if (!ActiveGames.TryGetValue(gameToJoin, out game))
            {
                return null;
            }

            var player = new Player(MasterServer.Instance.Clients[playerToJoin]);

            foreach (var type in deckInfo)
            {
                var card = Card.CreateFromTypeInfo(type);
                player.Deck.Cards.Push(card);
            }

            game.Players.Add(player);
            game.StartGame();

            SendUpdateToPlayers(game.Players.Where(x => !x.Id.Equals(player.Id)), game);

            return game;
        }

        public GameField AddToBench(NetworkId gameId, NetworkId playerId, List<NetworkId> pokemonIds)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var pokemons = pokemonIds.Select(id => (PokemonCard)game.FindCardById(id));
            game.OnBenchPokemonSelected(game.Players.First(p => p.Id.Equals(playerId)), pokemons);

            SendUpdateToPlayers(game.Players, game);

            return game;
        }

        public GameField EndTurn(NetworkId gameId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            game.EndTurn();

            SendUpdateToPlayers(game.Players, game);

            return game;
        }

        public GameField SetActivePokemon(NetworkId gameId, NetworkId playerId, NetworkId pokemonId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            game.OnActivePokemonSelected(playerId, (PokemonCard)game.FindCardById(pokemonId));

            SendUpdateToPlayers(game.Players, game);

            return game;
        }

        public GameField AttachEnergy(NetworkId gameId, NetworkId targetId, NetworkId energyCardId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var energyCard = (EnergyCard)game.FindCardById(energyCardId);
            var target = (PokemonCard)game.FindCardById(targetId);

            game.ActivePlayer.AttachEnergyToPokemon(energyCard, target, game);

            SendUpdateToPlayers(game.Players, game);

            return game;
        }

        public GameField ActivateAbility(NetworkId gameId, NetworkId abilityId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var ability = game.FindAbilityById(abilityId);
            game.ActivateAbility(ability);

            SendUpdateToPlayers(game.Players, game);
            return game;
        }

        public GameField Attack(NetworkId gameId, NetworkId attackId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var attack = game.FindAttackById(attackId);

            game.Attack(attack);

            SendUpdateToPlayers(game.Players, game);
            return game;
        }

        public GameField PlayCard(NetworkId gameId, NetworkId cardId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var card = game.FindCardById(cardId);

            if (card is TrainerCard)
            {
                game.PlayerTrainerCard((TrainerCard)card);
            }
            else if (card is PokemonCard)
            {
                game.PlayPokemon((PokemonCard)card);
            }

            SendUpdateToPlayers(game.Players, game);
            return game;
        }

        public GameField EvolvePokemon(NetworkId gameId, NetworkId basePokemonId, NetworkId targetPokemonId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var card = (PokemonCard)game.FindCardById(basePokemonId);
            var target = (PokemonCard)game.FindCardById(targetPokemonId);

            game.EvolvePokemon(card, target);

            SendUpdateToPlayers(game.Players, game);
            return game;
        }

        public GameField RetreatPokemon(NetworkId gameId, NetworkId targetPokemon, List<NetworkId> energyCardIds)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            var energyCards = energyCardIds.Select(cardId => (EnergyCard)game.FindCardById(cardId)).ToList();

            game.OnPokemonRetreated((PokemonCard)game.FindCardById(targetPokemon), energyCards);

            return game;
        }

        public GameField LeaveGame(NetworkId playerId, NetworkId gameId)
        {
            GameField game;

            if (!ActiveGames.TryGetValue(gameId, out game))
            {
                return null;
            }

            if (game.GameState == GameFieldState.GameOver)
            {
                return game;
            }

            game.GameState = GameFieldState.GameOver;
            SendUpdateToPlayers(game.Players, game);

            return game;
        }

        private void SendUpdateToPlayers(IEnumerable<Player> players, GameField game)
        {
            foreach (var player in players)
            {
                var gameMessage = new GameFieldMessage(game);
                player.NetworkPlayer.Send(gameMessage.ToNetworkMessage(MasterServer.Instance.Id));
            }
        }

        private void RemoveCompletedGames()
        {
            var games = ActiveGames.Values.Where(game => game.GameState == GameFieldState.GameOver).ToArray();

            foreach (var game in games)
            {
                ActiveGames.TryRemove(game.Id, out GameField _);
            }
        }
    }
}
