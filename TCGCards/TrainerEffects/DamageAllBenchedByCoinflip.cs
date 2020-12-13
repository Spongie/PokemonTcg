using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageAllBenchedByCoinflip : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Damage benched by flip";
            }
        }

        private int damageToOwnBenchIfTails;
        private int damageToOpponentBenchIfHeads;

        [DynamicInput("Damage to oppponents bench if heads")]
        public int DamageToOpponentBenchIfHeads
        {
            get { return damageToOpponentBenchIfHeads; }
            set
            {
                damageToOpponentBenchIfHeads = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to your bench if tails")]
        public int DamageToOwnBenchIfTails
        {
            get { return damageToOwnBenchIfTails; }
            set
            {
                damageToOwnBenchIfTails = value;
                FirePropertyChanged();
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
            if (game.FlipCoins(1) == 1)
            {
                foreach (var pokemon in opponent.BenchedPokemon)
                {
                    pokemon.DealDamage(DamageToOpponentBenchIfHeads, game, pokemonSource, true);
                }
            }
            else
            {
                foreach (var pokemon in caster.BenchedPokemon)
                {
                    pokemon.DealDamage(DamageToOwnBenchIfTails, game, pokemonSource, true);
                }
            }
        }
    }
}
