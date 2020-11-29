using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Tests
{
    class TestAttack : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }
    }

    class TestPokemon : PokemonCard
    {
        public TestPokemon(Player owner) : base(owner)
        {
        }
    }

    [TestClass()]
    public class AttackTests
    {
        [TestMethod()]
        public void CanBeUsed_NoAttachedEnergy()
        {
            var game = createTestGame();

            var pokemon = game.ActivePlayer.ActivePokemonCard;
            pokemon.Attacks = new ObservableCollection<Attack>
            {
                new TestAttack
                {
                    Cost = new ObservableCollection<Energy>
                    {
                        new Energy(EnergyTypes.Colorless, 1),
                        new Energy(EnergyTypes.Fire, 1)
                    }
                }
            };

            Assert.IsFalse(pokemon.Attacks.First().CanBeUsed(game, game.ActivePlayer, game.NonActivePlayer));
        }

        [TestMethod()]
        public void CanBeUsed_EnoughAttached_Colorless_Cost()
        {
            var game = createTestGame();

            var pokemon = game.ActivePlayer.ActivePokemonCard;
            pokemon.Attacks = new ObservableCollection<Attack>
            {
                new TestAttack
                {
                    Cost = new ObservableCollection<Energy>
                    {
                        new Energy(EnergyTypes.Colorless, 1),
                        new Energy(EnergyTypes.Fire, 1)
                    }
                }
            };
            pokemon.AttachedEnergy = new List<EnergyCard>
            {
                new EnergyCard() { EnergyType = EnergyTypes.Fire, Amount = 1 },
                new EnergyCard() { EnergyType = EnergyTypes.Fire, Amount = 1 }
            };

            Assert.IsTrue(pokemon.Attacks.First().CanBeUsed(game, game.ActivePlayer, game.NonActivePlayer));
        }

        [TestMethod()]
        public void CanBeUsed_EnoughAttached_FirstTurn()
        {
            var game = createTestGame();
            game.FirstTurn = true;

            var pokemon = game.ActivePlayer.ActivePokemonCard;
            pokemon.Attacks = new ObservableCollection<Attack>
            {
                new TestAttack
                {
                    Cost = new ObservableCollection<Energy>
                    {
                        new Energy(EnergyTypes.Colorless, 1),
                        new Energy(EnergyTypes.Fire, 1)
                    }
                }
            };
            pokemon.AttachedEnergy = new List<EnergyCard>
            {
                new EnergyCard() { EnergyType = EnergyTypes.Fire, Amount = 1 },
                new EnergyCard() { EnergyType = EnergyTypes.Fire, Amount = 1 }
            };

            Assert.IsFalse(pokemon.Attacks.First().CanBeUsed(game, game.ActivePlayer, game.NonActivePlayer));
        }

        [TestMethod()]
        public void CanBeUsed_EnoughAttached_Colorless_Cost_Double()
        {
            var game = createTestGame();

            var pokemon = game.ActivePlayer.ActivePokemonCard;
            pokemon.Attacks = new ObservableCollection<Attack>
            {
                new TestAttack
                {
                    Cost = new ObservableCollection<Energy>
                    {
                        new Energy(EnergyTypes.Colorless, 2),
                        new Energy(EnergyTypes.Fire, 1)
                    }
                }
            };
            pokemon.AttachedEnergy = new List<EnergyCard>
            {
                new EnergyCard() { EnergyType = EnergyTypes.Fire, Amount = 1 },
                new EnergyCard() { EnergyType = EnergyTypes.Colorless, Amount = 2 }
            };

            Assert.IsTrue(pokemon.Attacks.First().CanBeUsed(game, game.ActivePlayer, game.NonActivePlayer));
        }

        [TestMethod()]
        public void CanBeUsed_EnoughAttached_NoColorless_Attached()
        {
            var game = createTestGame();

            var pokemon = game.ActivePlayer.ActivePokemonCard;
            pokemon.Attacks = new ObservableCollection<Attack>
            {
                new TestAttack
                {
                    Cost = new ObservableCollection<Energy>
                    {
                        new Energy(EnergyTypes.Colorless, 1),
                        new Energy(EnergyTypes.Fire, 1)
                    }
                }
            };
            pokemon.AttachedEnergy = new List<EnergyCard>
            {
                new EnergyCard() { EnergyType = EnergyTypes.Fire, Amount = 1 },
                new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 }
            };

            Assert.IsTrue(pokemon.Attacks.First().CanBeUsed(game, game.ActivePlayer, game.NonActivePlayer));
        }

        private GameField createTestGame()
        {
            var game = new GameField();
            game.FirstTurn = false;
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(new Player() { Id = NetworkId.Generate() });

            game.ActivePlayer = game.Players.First();
            game.NonActivePlayer = game.Players.Last();

            game.ActivePlayer.ActivePokemonCard = new TestPokemon(game.ActivePlayer);
            game.NonActivePlayer.ActivePokemonCard = new TestPokemon(game.NonActivePlayer);

            return game;
        }
    }
}