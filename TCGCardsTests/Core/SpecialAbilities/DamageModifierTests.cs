using System.Collections.ObjectModel;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;
using Xunit;

namespace TCGCardsTests.Core.SpecialAbilities
{
    public class DamageModifierTests
    {
        [Fact]
        public void NewDamageApplied()
        {
            var attack = new Attack()
            {
                Damage = 20
            };

            attack.DamageModifier = new DamageModifier(40, 2);

            Assert.Equal(40, attack.Damage);
        }

        [Fact]
        public void NewDamageNotClearedFirstTime()
        {
            var attack = new Attack()
            {
                Damage = 20
            };

            attack.DamageModifier = new DamageModifier(40, 3);

            var pokemon = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            pokemon.EndTurn(new GameField()); //Turn ended By Attack
            pokemon.EndTurn(new GameField()); //Turn ended by opponent

            Assert.Equal(40, attack.Damage);
        }

        [Fact]
        public void NewDamageCleared()
        {
            var attack = new Attack()
            {
                Damage = 20
            };

            attack.DamageModifier = new DamageModifier(40, 3);

            var pokemon = new PokemonCard()
            {
                Attacks = new ObservableCollection<Attack>
                {
                    attack
                }
            };

            pokemon.EndTurn(new GameField()); //Turn ended By Attack
            pokemon.EndTurn(new GameField()); //Turn ended by opponent
            pokemon.EndTurn(new GameField()); //Turn ended by new attack

            Assert.Equal(20, attack.Damage);
        }
    }
}
