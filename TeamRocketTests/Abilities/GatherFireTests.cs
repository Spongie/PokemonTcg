using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class GatherFireTests
    {
        [TestMethod]
        public void Trigger()
        {
            var player = new Player();
            player.BenchedPokemon.Add(new Charmander(player));
            player.BenchedPokemon.First().AttachedEnergy.Add(new FireEnergy());

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<TCGCards.Card>
            {
                player.BenchedPokemon.First().AttachedEnergy.First()
            }));

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new Charmander(player);

            var ability = new GatherFire(player.ActivePokemonCard);

            ability.Trigger(player, null, 0);

            Assert.AreEqual(0, player.BenchedPokemon.First().AttachedEnergy.Count);
            Assert.AreEqual(1, player.ActivePokemonCard.AttachedEnergy.Count);
        }
    }
}
