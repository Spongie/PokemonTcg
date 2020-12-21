using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class SurpriseThunder : IEffect
    {
        public string EffectType
        {
            get
            {
                return "Surpirse Thunder";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (game.FlipCoins(1) == 0)
            {
                return;
            }

            var damage = game.FlipCoins(1) == 1 ? 20 : 10;

            foreach (var pokemon in opponent.BenchedPokemon.ValidPokemonCards)
            {
                pokemon.DealDamage(damage, game, pokemonSource, false);
            }
        }
    }
}
