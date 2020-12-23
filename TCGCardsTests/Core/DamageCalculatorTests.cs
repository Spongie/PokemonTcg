﻿using Xunit;
using Entities;

namespace TCGCards.Core.Tests
{
    public class DamageCalculatorTests
    {
        [Fact]
        public void GetDamageAfterWeaknessAndResistance_No_Attack()
        {
            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, null, null, null);

            Assert.Equal(30, result);
        }

        [Fact]
        public void GetDamageAfterWeaknessAndResistance_Wrong_Weakness()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Weakness = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.Equal(30, result);
        }

        [Fact]
        public void GetDamageAfterWeaknessAndResistance_Weakness()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Fire };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Weakness = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.Equal(60, result);
        }

        [Fact]
        public void GetDamageAfterWeaknessAndResistance_Weakness_Dont_Apply()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Fire };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Weakness = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = false };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.Equal(30, result);
        }

        [Fact]
        public void GetDamageAfterWeaknessAndResistance_Wrong_Resistance()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Resistance = EnergyTypes.Fire };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.Equal(30, result);
        }

        [Fact]
        public void GetDamageAfterWeaknessAndResistance_Resistance()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Resistance = EnergyTypes.Grass };
            var attack = new Attack() { ApplyResistance = true, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetDamageAfterWeaknessAndResistance_Resistance_Dont_Apply()
        {
            var attacker = new PokemonCard() { Type = EnergyTypes.Grass };
            var defender = new PokemonCard() { Type = EnergyTypes.Grass, Resistance = EnergyTypes.Grass };
            var attack = new Attack() { ApplyResistance = false, ApplyWeakness = true };

            var result = DamageCalculator.GetDamageAfterWeaknessAndResistance(30, attacker, defender, attack);

            Assert.Equal(30, result);
        }
    }
}