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
            p.DiscardPile.Add(new Oddish(p) { IsRevealed = true });

            theOnlyActiveGame.Players.Add(p);

            return theOnlyActiveGame;
        }

        public GameField JoinTheActiveGame(NetworkId playerToJoin)
        {
            var p = new Player(MasterServer.Instance.Clients[playerToJoin]);
            p.DiscardPile.Add(new Oddish(p) { IsRevealed = true });
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

        public GameField AddToBench(NetworkId playerId, List<NetworkId> pokemonIds)
        {
            var pokemons = pokemonIds.Select(id => (PokemonCard)theOnlyActiveGame.FindCardById(id));
            theOnlyActiveGame.OnBenchPokemonSelected(theOnlyActiveGame.Players.First(p => p.Id.Equals(playerId)), pokemons);

            SendUpdateToPlayers(theOnlyActiveGame.Players);

            return theOnlyActiveGame;
        }

        public GameField EndTurn()
        {
            theOnlyActiveGame.EndTurn();

            SendUpdateToPlayers(theOnlyActiveGame.Players);

            return theOnlyActiveGame;
        }

        public GameField SetActivePokemon(NetworkId playerId, NetworkId pokemonId)
        {
            theOnlyActiveGame.OnActivePokemonSelected(playerId, (PokemonCard)theOnlyActiveGame.FindCardById(pokemonId));

            SendUpdateToPlayers(theOnlyActiveGame.Players);

            return theOnlyActiveGame;
        }
        //TODO Fixa så att ID används och sen leta uppp rätt kort...
        public GameField AttachEnergy(NetworkId targetId, NetworkId energyCardId)
        {
            var energyCard = (EnergyCard)theOnlyActiveGame.FindCardById(energyCardId);
            var target = (PokemonCard)theOnlyActiveGame.FindCardById(targetId);

            theOnlyActiveGame.ActivePlayer.AttachEnergyToPokemon(energyCard, target);

            SendUpdateToPlayers(theOnlyActiveGame.Players);

            return theOnlyActiveGame;
        }

        public GameField ActivateAbility(NetworkId abilityId)
        {
            var ability = theOnlyActiveGame.FindAbilityById(abilityId);
            theOnlyActiveGame.ActivateAbility(ability);

            SendUpdateToPlayers(theOnlyActiveGame.Players);
            return theOnlyActiveGame;
        }

        public GameField Attack(NetworkId attackId)
        {
            var attack = theOnlyActiveGame.FindAttackById(attackId);

            theOnlyActiveGame.Attack(attack);

            SendUpdateToPlayers(theOnlyActiveGame.Players);
            return theOnlyActiveGame;
        }

        public GameField PlayCard(NetworkId cardId)
        {
            var card = theOnlyActiveGame.FindCardById(cardId);

            if (card is TrainerCard)
            {
                theOnlyActiveGame.PlayerTrainerCard((TrainerCard)card);
            }
            else if (card is PokemonCard)
            {
                theOnlyActiveGame.PlayPokemon((PokemonCard)card);
            }

            SendUpdateToPlayers(theOnlyActiveGame.Players);
            return theOnlyActiveGame;
        }

        public GameField EvolvePokemon(NetworkId basePokemonId, NetworkId targetPokemonId)
        {
            var card = (PokemonCard)theOnlyActiveGame.FindCardById(basePokemonId);
            var target = (PokemonCard)theOnlyActiveGame.FindCardById(targetPokemonId);

            theOnlyActiveGame.EvolvePokemon(card, target);

            SendUpdateToPlayers(theOnlyActiveGame.Players);
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
