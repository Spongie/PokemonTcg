using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class PokemonCenter : TrainerCard
    {
        public PokemonCenter()
        {
            Name = "Pokémon Center";
            Description = "Remove all damage counters from your pokémon and then discard all their attached energy";
            Set = Singleton.Get<Set>();
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            HealAndDiscardEnergy(caster.ActivePokemonCard);

            foreach (var pokemon in caster.BenchedPokemon)
            {
                HealAndDiscardEnergy(pokemon);
            }
        }

        private static void HealAndDiscardEnergy(PokemonCard pokemon)
        {
            if (pokemon.DamageCounters == 0)
            {
                return;
            }

            pokemon.DamageCounters = 0;
            var cardsToDiscard = pokemon.AttachedEnergy.ToArray();

            foreach (var card in cardsToDiscard)
            {
                pokemon.DiscardEnergyCard(card);
            }
        }
    }
}
