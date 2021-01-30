using Xunit;
using Entities;
using System.Collections.ObjectModel;
using TCGCards.Core;

namespace TCGCards.EnergyCards.Tests
{
    public class BuzzardEnergyTests
    {
        [Fact()]
        public void BuzzardEnergyTest()
        {
            var attack = new Attack
            {
                Cost = new ObservableCollection<Energy>
                {
                    new Energy(EnergyTypes.Electric, 1),
                    new Energy(EnergyTypes.Colorless, 1)
                }
            };

            var owner = new Player();
            var pokemon = new PokemonCard(owner)
            {
                Attacks = new ObservableCollection<Attack> { attack }
            };
            owner.ActivePokemonCard = pokemon;

            pokemon.AttachedEnergy.Add(new BuzzardEnergy(pokemon, EnergyTypes.Electric));

            Assert.True(attack.CanBeUsed(new GameField() { FirstTurn = false }, owner, new Player()));
        }
    }
}