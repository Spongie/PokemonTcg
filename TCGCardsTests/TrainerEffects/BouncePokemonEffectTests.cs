using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.TrainerEffects;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using NSubstitute;
using NetworkingCore;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects.Tests
{
    [TestClass()]
    public class BouncePokemonEffectTests
    {
        [TestMethod()]
        public void CanBeUsed_Bench()
        {
            var effect = new BouncePokemonEffect()
            {
                ShuffleIntoDeck = true,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();
            var pokemon = new PokemonCard(opponent)
            {
                AttachedEnergy = new List<EnergyCard>
                {
                    new EnergyCard()
                }
            };

            opponent.ActivePokemonCard = pokemon;
            var other = new PokemonCard(opponent);
            opponent.BenchedPokemon.Add(other);

            Assert.IsTrue(effect.CanCast(new GameField(), null, opponent));
        }

        [TestMethod()]
        public void CanBeUsed_EmptyBench()
        {
            var effect = new BouncePokemonEffect()
            {
                ShuffleIntoDeck = true,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();
            var pokemon = new PokemonCard(opponent)
            {
                AttachedEnergy = new List<EnergyCard>
                {
                    new EnergyCard()
                }
            };

            opponent.ActivePokemonCard = pokemon;

            Assert.IsFalse(effect.CanCast(new GameField(), null, opponent));
        }

        [TestMethod()]
        public void CardShuffledIntoDeck()
        {
            var effect = new BouncePokemonEffect()
            {
                ShuffleIntoDeck = true,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();
            var pokemon = new PokemonCard(opponent)
            {
                AttachedEnergy = new List<EnergyCard>
                {
                    new EnergyCard()
                }
            };

            opponent.ActivePokemonCard = pokemon;
            var other = new PokemonCard(opponent);
            opponent.BenchedPokemon.Add(other);

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage
            {
                Cards = new List<NetworkId> { other.Id }
            });
            opponent.SetNetworkPlayer(sub);
            
            effect.Process(new GameField(), new Player(), opponent);

            Assert.AreEqual(2, opponent.Deck.Cards.Count);
            Assert.AreEqual(other, opponent.ActivePokemonCard);
        }

        [TestMethod()]
        public void CardShuffledIntoHand()
        {
            var effect = new BouncePokemonEffect()
            {
                ShuffleIntoDeck = false,
                ReturnAttachedToHand = true,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();
            var pokemon = new PokemonCard(opponent)
            {
                AttachedEnergy = new List<EnergyCard>
                {
                    new EnergyCard()
                }
            };

            opponent.ActivePokemonCard = pokemon;
            var other = new PokemonCard(opponent);
            opponent.BenchedPokemon.Add(other);

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage
            {
                Cards = new List<NetworkId> { other.Id }
            });
            opponent.SetNetworkPlayer(sub);

            effect.Process(new GameField(), new Player(), opponent);

            Assert.AreEqual(2, opponent.Hand.Count);
            Assert.AreEqual(other, opponent.ActivePokemonCard);
        }
    }
}