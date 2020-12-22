﻿using TCGCards;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Abilities;

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

        [TestMethod()]
        public void HaveEnoughEnergy_Missing_One()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>()
                {
                    new Energy(EnergyTypes.Fire, 4)
                }
            };

            var player = new Player
            {
                ActivePokemonCard = new PokemonCard
                {
                    AttachedEnergy = new List<EnergyCard>
                    {
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                    }
                }
            };

            Assert.IsFalse(attack.HaveEnoughEnergy(player));
        }

        [TestMethod()]
        public void HaveEnoughEnergy_One_Wrong()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>()
                {
                    new Energy(EnergyTypes.Fire, 4)
                }
            };

            var player = new Player
            {
                ActivePokemonCard = new PokemonCard
                {
                    AttachedEnergy = new List<EnergyCard>
                    {
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Water},
                    }
                }
            };

            Assert.IsFalse(attack.HaveEnoughEnergy(player));
        }

        [TestMethod()]
        public void HaveEnoughEnergy_All_Same()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>()
                {
                    new Energy(EnergyTypes.Fire, 4)
                }
            };

            var player = new Player
            {
                ActivePokemonCard = new PokemonCard
                {
                    AttachedEnergy = new List<EnergyCard>
                    {
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                    }
                }
            };

            Assert.IsTrue(attack.HaveEnoughEnergy(player));
        }

        [TestMethod()]
        public void HaveEnoughEnergy_With_Colorless_Cost()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>()
                {
                    new Energy(EnergyTypes.Fire, 3),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var player = new Player
            {
                ActivePokemonCard = new PokemonCard
                {
                    AttachedEnergy = new List<EnergyCard>
                    {
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                    }
                }
            };

            Assert.IsTrue(attack.HaveEnoughEnergy(player));
        }

        [TestMethod()]
        public void HaveEnoughEnergy_With_Override()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>()
                {
                    new Energy(EnergyTypes.Fire, 4)
                }
            };

            var player = new Player
            {
                ActivePokemonCard = new PokemonCard
                {
                    AttachedEnergy = new List<EnergyCard>
                    {
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Water},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Water},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Fire},
                    },
                    TemporaryAbilities = new List<Ability>
                    {
                        new EnergyTypeOverrideTemporaryAbility()
                        {
                            NewType = EnergyTypes.Fire,
                            SourceType = EnergyTypes.All
                        }
                    }
                }
            };

            Assert.IsTrue(attack.HaveEnoughEnergy(player));
        }

        [TestMethod()]
        public void HaveEnoughEnergy_With_Override_Double_Colorless()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>()
                {
                    new Energy(EnergyTypes.Fire, 4)
                }
            };

            var player = new Player
            {
                ActivePokemonCard = new PokemonCard
                {
                    AttachedEnergy = new List<EnergyCard>
                    {
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Water},
                        new EnergyCard { Amount = 1, EnergyType = EnergyTypes.Water},
                        new EnergyCard { Amount = 2, EnergyType = EnergyTypes.Colorless},
                    },
                    TemporaryAbilities = new List<Ability>
                    {
                        new EnergyTypeOverrideTemporaryAbility()
                        {
                            NewType = EnergyTypes.Fire,
                            SourceType = EnergyTypes.All
                        }
                    }
                }
            };

            Assert.IsTrue(attack.HaveEnoughEnergy(player));
        }
    }
}