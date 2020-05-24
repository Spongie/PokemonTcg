using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Earthquake : Attack
    {
        public Earthquake()
        {
            Name = "Earthquake";
            Description = "Does 10 damage to each of your own Benched Pokémon. (Don't apply Weakness and Resistance for Benched Pokémon.)";
			DamageText = "70";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 70;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            foreach (var pokemon in owner.BenchedPokemon)
            {
                pokemon.DamageCounters += 10;
            }
        }
    }
}
