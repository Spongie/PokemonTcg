using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<TCGCards.Card>
            {
                new Oddish(player),
                new Oddish(player)
            }));

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new DarkDragonite(player);

            var ability = new SummonMinions(player.ActivePokemonCard);

            ability.Trigger(player, null, 0);

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
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<TCGCards.Card>
            {
                new Oddish(player)
            }));

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new DarkDragonite(player);

            var ability = new SummonMinions(player.ActivePokemonCard);

            ability.Trigger(player, null, 0);

            networkPlayer.DidNotReceive().SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>());
        }
    }
}
