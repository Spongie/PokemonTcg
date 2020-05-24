using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;
using TeamRocket.Attacks;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Attacks
{
    [TestClass]
    public class MagneticLinesTests
    {
        [TestMethod]
        public void Test_Defending_No_Energy()
        {
            var game = new GameField();
            game.IgnorePostAttack = true;
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMagneton(owner);
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

            game.Attack(owner.ActivePokemonCard.Attacks.OfType<MagneticLines>().First());

            Assert.AreEqual(30, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Test_Defending_Has_Energy_No_Benched()
        {
            var game = new GameField();
            game.IgnorePostAttack = true;
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMagneton(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkPrimeape(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            INetworkPlayer networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                opponent.ActivePokemonCard.Id
            }));

            owner.SetNetworkPlayer(networkPlayer);

            game.Attack(owner.ActivePokemonCard.Attacks.OfType<MagneticLines>().First());

            Assert.AreEqual(30, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Test_Defending_Has_Energy_One_Benched()
        {
            var game = new GameField();
            game.IgnorePostAttack = true;
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMagneton(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkPrimeape(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            opponent.BenchedPokemon.Add(new DarkPersian(opponent));

            INetworkPlayer networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(Arg.Is<NetworkMessage>(x => x.MessageType == MessageTypes.SelectFromOpponentBench)).Returns(new CardListMessage(new List<NetworkId>
            {
                opponent.BenchedPokemon[0].Id
            }));

            networkPlayer.SendAndWaitForResponse<CardListMessage>(Arg.Is<NetworkMessage>(x => x.MessageType == MessageTypes.PickFromList)).Returns(new CardListMessage(new List<NetworkId>
            {
                opponent.ActivePokemonCard.AttachedEnergy.First().Id
            }));

            owner.SetNetworkPlayer(networkPlayer);

            game.Attack(owner.ActivePokemonCard.Attacks.OfType<MagneticLines>().First());

            Assert.AreEqual(30, opponent.ActivePokemonCard.DamageCounters);

            Assert.AreEqual(0, opponent.ActivePokemonCard.AttachedEnergy.Count);
            Assert.AreEqual(1, opponent.BenchedPokemon[0].AttachedEnergy.Count);
        }
    }
}
