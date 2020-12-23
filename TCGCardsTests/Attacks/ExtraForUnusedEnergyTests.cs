using Xunit;
using TCGCards.Attacks;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Collections.ObjectModel;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class ExtraForUnusedEnergyTests
    {
        [Fact]
        public void GetDamageTest_1_Energy()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(10, damage.NormalDamage);
        }

        [Fact]
        public void GetDamageTest_1_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(20, damage.NormalDamage);
        }

        [Fact]
        public void GetDamageTest_2_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(30, damage.NormalDamage);
        }

        [Fact]
        public void GetDamageTest_4_Extra_Limited()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(30, damage.NormalDamage);
        }

        [Fact]
        public void GetDamageTest_1_Energy_With_Colorless_Cost()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Fire });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(10, damage.NormalDamage);
        }

        [Fact]
        public void GetDamageTest_1_Extra_With_Colorless_Cost()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Fire });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(20, damage.NormalDamage);
        }

        [Fact]
        public void GetDamageTest_1_Extra_Wrong_With_Colorless_Cost()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 10,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Fire });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Fire });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(10, damage.NormalDamage);
        }

        [Fact]
        public void Blastoise_0_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 40,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 3)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(40, damage.NormalDamage);
        }

        [Fact]
        public void Blastoise_1_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 40,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 3)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(50, damage.NormalDamage);
        }

        [Fact]
        public void Blastoise_2_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 40,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 3)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(60, damage.NormalDamage);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(14)]
        public void Blastoise_4_Extra(int attachedEnergy)
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 40,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 3)
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            for (int i = 0; i < attachedEnergy; i++)
            {
                owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            }

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(60, damage.NormalDamage);
        }

        [Fact]
        public void Seadra_0_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 20,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(20, damage.NormalDamage);
        }

        [Fact]
        public void Seadra_1_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 20,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(30, damage.NormalDamage);
        }

        [Fact]
        public void Seadra_1_Extra_With_Double()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 20,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 2, EnergyType = EnergyTypes.Colorless });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(30, damage.NormalDamage);
        }

        [Fact]
        public void Seadra_2_Extra()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 20,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(40, damage.NormalDamage);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void Seadra_4_Extra(int attachedEnergy)
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 20,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            for (int i = 0; i < attachedEnergy; i++)
            {
                owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            }

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(40, damage.NormalDamage);
        }

        [Fact]
        public void Seadra_1_Extra_Wrong_Type()
        {
            var attack = new ExtraForUnusedEnergy()
            {
                Damage = 20,
                MaxExtraDamage = 20,
                AmountPerEnergy = 10,
                EnergyType = EnergyTypes.Water,
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Water, 1),
                    new Energy(EnergyTypes.Colorless, 1),
                }
            };

            var owner = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 2, EnergyType = EnergyTypes.Colorless });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.Equal(20, damage.NormalDamage);
        }
    }
}