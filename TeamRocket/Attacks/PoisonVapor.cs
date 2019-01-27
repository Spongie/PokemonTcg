using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PoisonVapor : Attack
    {
        public PoisonVapor()
        {
            Name = "Poison Vapor";
            Description = "The Defending Pokémon is now Poisoned. This attack does 10 damage to each of your opponent's Benched Pokémon. (Don't apply Weakness and Resistance for Benched Pokémon.)";
            DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.IsPoisoned = true;

            foreach (var pokemon in opponent.BenchedPokemon)
            {
                pokemon.DamageCounters += 10;
            }
        }
    }
}
