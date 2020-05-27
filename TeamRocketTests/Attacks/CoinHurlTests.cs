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
    public class CoinHurlTests
    {
        [TestMethod]
        public void Test()
        {
            var game = new GameField();
            game.IgnorePostAttack = true;
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new Meowth(owner);
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

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(20, opponent.ActivePokemonCard.DamageCounters);
        }
    }
}
