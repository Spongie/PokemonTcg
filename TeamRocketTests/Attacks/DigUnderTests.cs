using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Attacks
{
    [TestClass]
    public class DigUnderTests
    {
        [TestMethod]
        public void Test()
        {
            var game = new GameField();
            game.IgnorePostAttack = true;
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new Diglett(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkPrimeape(opponent);

            INetworkPlayer networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                opponent.ActivePokemonCard.Id
            }));

            owner.SetNetworkPlayer(networkPlayer);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(10, opponent.ActivePokemonCard.DamageCounters);
        }
    }
}
