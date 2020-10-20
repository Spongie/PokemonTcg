using System.Collections.Generic;
using System.Linq;
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
            var energyCard = new FireEnergy();
            player.BenchedPokemon[0].AttachedEnergy.Add(energyCard);

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                player.BenchedPokemon[0].Id
            }));

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new Charmander(player);

            var ability = new GatherFire(player.ActivePokemonCard);

            ability.Trigger(player, null, 0, new GameLog());

            Assert.AreEqual(0, player.BenchedPokemon.First().AttachedEnergy.Count);
            Assert.AreEqual(1, player.ActivePokemonCard.AttachedEnergy.Count);
        }
    }
}
