using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class SummonMinionsTests
    {
        [TestMethod]
        public void Trigger_Test_EmptyBench()
        {
            var player = new Player();

            var card1 = new Oddish(player);
            var card2 = new Oddish(player);

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                card1.Id,
                card2.Id
            }));

            player.Deck.Cards.Push(card1);
            player.Deck.Cards.Push(card2);

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new DarkDragonite(player);

            var ability = new SummonMinions(player.ActivePokemonCard);

            ability.Trigger(player, null, 0, new GameLog());

            Assert.AreEqual(2, player.BenchedPokemon.Count);
        }

        [TestMethod]
        public void Trigger_Test_FullBench()
        {
            var player = new Player();
            player.BenchedPokemon.Add(new Oddish(player));
            player.BenchedPokemon.Add(new Oddish(player));
            player.BenchedPokemon.Add(new Oddish(player));
            player.BenchedPokemon.Add(new Oddish(player));
            player.BenchedPokemon.Add(new Oddish(player));

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                new Oddish(player).Id
            }));

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new DarkDragonite(player);

            var ability = new SummonMinions(player.ActivePokemonCard);

            ability.Trigger(player, null, 0, new GameLog());

            networkPlayer.DidNotReceive().SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>());
        }
    }
}
