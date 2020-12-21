using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Attacks;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Collections.ObjectModel;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class ExtraForUnusedEnergyTests
    {
        [TestMethod()]
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

            Assert.AreEqual(10, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(20, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(30, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(30, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(10, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(20, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(10, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(40, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(50, damage.NormalDamage);
        }

        [TestMethod()]
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

            Assert.AreEqual(60, damage.NormalDamage);
        }

        [TestMethod()]
        public void Blastoise_4_Extra()
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
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });
            owner.AttachedEnergy.Add(new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Water });

            var player = new Player();
            player.ActivePokemonCard = owner;

            var damage = attack.GetDamage(player, null, null);

            Assert.AreEqual(60, damage.NormalDamage);
        }
    }
}