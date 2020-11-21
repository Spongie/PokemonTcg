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

            effect.Process(new GameField(), new Player(), opponent, null);

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

            effect.Process(new GameField(), new Player(), opponent, null);

            Assert.AreEqual(2, opponent.Hand.Count);
            Assert.AreEqual(other, opponent.ActivePokemonCard);
        }

        [TestMethod()]
        public void Bounce_EvolvedPokemon()
        {
            var effect = new BouncePokemonEffect()
            {
                ShuffleIntoDeck = false,
                ReturnAttachedToHand = true,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();

            var pokemon = new PokemonCard(opponent) { Name = "Pokemon1", Stage = 0 };
            var evolution = new PokemonCard(opponent) { Name = "Evo", Stage = 1, EvolvesFrom = "Pokemon1" };
            opponent.ActivePokemonCard = evolution;

            pokemon.Evolve(evolution);
            var other = new PokemonCard(opponent);
            opponent.BenchedPokemon.Add(other);

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage
            {
                Cards = new List<NetworkId> { other.Id }
            });
            opponent.SetNetworkPlayer(sub);

            effect.Process(new GameField(), new Player(), opponent, null);

            Assert.AreEqual(2, opponent.Hand.Count);
        }

        [TestMethod()]
        public void Bounce_EvolvedPokemon_OnlyBasic()
        {
            var effect = new BouncePokemonEffect()
            {
                OnlyBasic = true,
                ShuffleIntoDeck = false,
                ReturnAttachedToHand = true,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();

            var pokemon = new PokemonCard(opponent) { Name = "Pokemon1", Stage = 0 };
            var evolution = new PokemonCard(opponent) { Name = "Evo", Stage = 1, EvolvesFrom = "Pokemon1" };
            opponent.ActivePokemonCard = evolution;

            pokemon.Evolve(evolution);
            var other = new PokemonCard(opponent);
            opponent.BenchedPokemon.Add(other);

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage
            {
                Cards = new List<NetworkId> { other.Id }
            });
            opponent.SetNetworkPlayer(sub);

            effect.Process(new GameField(), new Player(), opponent, null);

            Assert.AreEqual(1, opponent.Hand.Count);
            Assert.AreEqual(pokemon.Id, opponent.Hand[0].Id);

            Assert.AreEqual(1, opponent.DiscardPile.Count);
            Assert.AreEqual(evolution.Id, opponent.DiscardPile[0].Id);
        }
    }
}