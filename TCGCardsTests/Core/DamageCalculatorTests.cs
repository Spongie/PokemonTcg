using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace TCGCards.Core.Tests
{
    [TestClass]
    public class DamageCalculatorTests
    {
        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_No_Attack()
        {
            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, null, null, null);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_Wrong_Weakness()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Weakness = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_Weakness()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Fire };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Weakness = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.AreEqual(60, result);
        }

        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_Weakness_Dont_Apply()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Fire };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Weakness = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = false };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_Wrong_Resistance()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Resistance = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_Resistance()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Resistance = EnergyTypes.Grass };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetDamageAfterWeaknessAndResistance_Resistance_Dont_Apply()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Resistance = EnergyTypes.Grass };
            var attack = new Attack() { ApplyResistance = false, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.AreEqual(30, result);
        }
    }
}