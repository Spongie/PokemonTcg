using Xunit;
using System.Collections.Generic;
using TCGCards.Core;
using NSubstitute;
using NetworkingCore;
using TCGCards.Core.Messages;
using System.Linq;

namespace TCGCards.TrainerEffects.Tests
{
    public class BouncePokemonEffectTests
    {
        [Fact]
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

            Assert.True(effect.CanCast(new GameField(), null, opponent));
        }

        [Fact]
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

            Assert.False(effect.CanCast(new GameField(), null, opponent));
        }

        [Fact]
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

            var game = new GameField();
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(opponent);
            opponent.Id = NetworkId.Generate();
            game.Cards.Add(other.Id, other);

            effect.Process(game, game.Players[0], opponent, null);

            Assert.Equal(2, opponent.Deck.Cards.Count);
            Assert.Equal(other, opponent.ActivePokemonCard);
        }

        [Fact]
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

            var game = new GameField();
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(opponent);
            opponent.Id = NetworkId.Generate();
            game.Cards.Add(other.Id, other);

            effect.Process(game, game.Players[0], opponent, null);

            Assert.Equal(2, opponent.Hand.Count);
            Assert.Equal(other, opponent.ActivePokemonCard);
        }

        [Fact]
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

            var game = new GameField();
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(opponent);
            opponent.Id = NetworkId.Generate();
            game.Cards.Add(other.Id, other);

            effect.Process(game, game.Players[0], opponent, null);

            Assert.Equal(2, opponent.Hand.Count);
        }

        [Fact]
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

            var game = new GameField();
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(opponent);
            opponent.Id = NetworkId.Generate();
            game.Cards.Add(other.Id, other);

            effect.Process(game, game.Players[0], opponent, null);

            Assert.Single(opponent.Hand);
            Assert.Equal(pokemon.Id, opponent.Hand[0].Id);

            Assert.Single(opponent.DiscardPile);
            Assert.Equal(evolution.Id, opponent.DiscardPile[0].Id);
        }

        [Fact]
        public void ScoopUp_TargetIsBasic()
        {
            var effect = new BouncePokemonEffect
            {
                OnlyBasic = true,
                TargetingMode = TargetingMode.YourPokemon,
                ReturnAttachedToHand = false,
                ShuffleIntoDeck = false
            };
            var game = new GameField();
            var player = new Player();

            var activePokemon = new PokemonCard() { Stage = 1, Owner = player };
            var targetPokemon = new PokemonCard()
            {
                Stage = 0,
                AttachedEnergy = new List<EnergyCard> { new EnergyCard(), new EnergyCard() },
                Owner = player
            };

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { targetPokemon.Id } });
            player.SetNetworkPlayer(sub);
            player.ActivePokemonCard = activePokemon;
            player.BenchedPokemon.Add(targetPokemon);

            
            game.ActivePlayer = player;
            game.NonActivePlayer = new Player();

            game.AddPlayer(player);

            effect.Process(game, player, game.NonActivePlayer, null);

            Assert.Equal(0, player.BenchedPokemon.Count);
            Assert.Equal(2, player.DiscardPile.Count);
            Assert.Single(player.Hand);
            Assert.Equal(targetPokemon.Id, player.Hand[0].Id);
        }

        [Fact]
        public void ScoopUp_TargetIsEvolved()
        {
            var effect = new BouncePokemonEffect
            {
                OnlyBasic = true,
                TargetingMode = TargetingMode.YourPokemon,
                ReturnAttachedToHand = false,
                ShuffleIntoDeck = false
            };

            var player = new Player();

            var activePokemon = new PokemonCard() { Stage = 1, Owner = player };
            var basicPokemon = new PokemonCard();
            var targetPokemon = new PokemonCard()
            {
                Stage = 1,
                AttachedEnergy = new List<EnergyCard> { new EnergyCard(), new EnergyCard() },
                Owner = player,
                EvolvedFrom = basicPokemon
            };

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { targetPokemon.Id } });
            player.SetNetworkPlayer(sub);
            player.ActivePokemonCard = activePokemon;
            player.BenchedPokemon.Add(targetPokemon);

            var game = new GameField();
            game.ActivePlayer = player;
            game.NonActivePlayer = new Player();

            game.AddPlayer(player);

            effect.Process(game, player, game.NonActivePlayer, null);

            Assert.Equal(0, player.BenchedPokemon.Count);
            Assert.Equal(3, player.DiscardPile.Count);
            Assert.Single(player.Hand);
            Assert.Equal(basicPokemon.Id, player.Hand[0].Id);
        }

        [Fact]
        public void ScoopUp_TargetIsEvolved_ToHand()
        {
            var effect = new BouncePokemonEffect
            {
                OnlyBasic = true,
                TargetingMode = TargetingMode.YourPokemon,
                ReturnAttachedToHand = true,
                ShuffleIntoDeck = false
            };

            var player = new Player();

            var activePokemon = new PokemonCard() { Stage = 1, Owner = player };
            var basicPokemon = new PokemonCard();
            var targetPokemon = new PokemonCard()
            {
                Stage = 1,
                AttachedEnergy = new List<EnergyCard> { new EnergyCard(), new EnergyCard() },
                Owner = player,
                EvolvedFrom = basicPokemon
            };

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { targetPokemon.Id } });
            player.SetNetworkPlayer(sub);
            player.ActivePokemonCard = activePokemon;
            player.BenchedPokemon.Add(targetPokemon);

            var game = new GameField();
            game.ActivePlayer = player;
            game.NonActivePlayer = new Player();

            game.AddPlayer(player);

            effect.Process(game, player, game.NonActivePlayer, null);

            Assert.Equal(0, player.BenchedPokemon.Count);
            Assert.Single(player.DiscardPile);
            Assert.Equal(3, player.Hand.Count);
        }

        [Fact]
        public void ScoopUp_TargetIsEvolved_AttachedToHand()
        {
            var effect = new BouncePokemonEffect
            {
                TargetingMode = TargetingMode.YourPokemon,
                ReturnAttachedToHand = true,
                ShuffleIntoDeck = false
            };

            var player = new Player();

            var activePokemon = new PokemonCard() { Stage = 1, Owner = player };
            var basicPokemon = new PokemonCard();
            var targetPokemon = new PokemonCard()
            {
                Stage = 1,
                AttachedEnergy = new List<EnergyCard> { new EnergyCard(), new EnergyCard() },
                Owner = player,
                EvolvedFrom = basicPokemon
            };

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { targetPokemon.Id } });
            player.SetNetworkPlayer(sub);
            player.ActivePokemonCard = activePokemon;
            player.BenchedPokemon.Add(targetPokemon);

            var game = new GameField();
            game.ActivePlayer = player;
            game.NonActivePlayer = new Player();

            game.AddPlayer(player);

            effect.Process(game, player, game.NonActivePlayer, null);

            Assert.Equal(0, player.BenchedPokemon.Count);
            Assert.Empty(player.DiscardPile);
            Assert.Equal(4, player.Hand.Count);
        }

        [Fact]
        public void ScoopUp_TargetIsEvolved_Shuffle()
        {
            var effect = new BouncePokemonEffect
            {
                TargetingMode = TargetingMode.YourPokemon,
                ReturnAttachedToHand = false,
                ShuffleIntoDeck = true
            };

            var player = new Player();

            var activePokemon = new PokemonCard() { Stage = 1, Owner = player };
            var basicPokemon = new PokemonCard();
            var targetPokemon = new PokemonCard()
            {
                Stage = 1,
                AttachedEnergy = new List<EnergyCard> { new EnergyCard(), new EnergyCard() },
                Owner = player,
                EvolvedFrom = basicPokemon
            };

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { targetPokemon.Id } });
            player.SetNetworkPlayer(sub);
            player.ActivePokemonCard = activePokemon;
            player.BenchedPokemon.Add(targetPokemon);

            var game = new GameField();
            game.ActivePlayer = player;
            game.NonActivePlayer = new Player();

            game.AddPlayer(player);

            effect.Process(game, player, game.NonActivePlayer, null);

            Assert.Equal(0, player.BenchedPokemon.Count);
            Assert.Empty(player.DiscardPile);
            Assert.Equal(4, player.Deck.Cards.Count);
            Assert.Empty(player.Hand);
        }
    }
}