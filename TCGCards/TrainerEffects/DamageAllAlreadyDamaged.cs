using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageAllAlreadyDamaged : DataModel, IEffect
    {
        private bool yourPokemon;
        private bool opponentPokemon;
        private int amount;

        public string EffectType
        {
            get
            {
                return "Damage already damaged";
            }
        }

        [DynamicInput("Your pokémon?", InputControl.Boolean)]
        public bool YourPokemon
        {
            get { return yourPokemon; }
            set
            {
                yourPokemon = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Opponents pokémon?", InputControl.Boolean)]
        public bool OpponentPokemon
        {
            get { return opponentPokemon; }
            set
            {
                opponentPokemon = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
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
            if (OpponentPokemon)
            {
                foreach (var pokemon in opponent.GetAllPokemonCards())
                {
                    if (pokemon.DamageCounters > 0)
                    {
                        pokemon.DealDamage(Amount, game, pokemon, true);
                    }
                }
            }

            if (yourPokemon)
            {
                foreach (var pokemon in caster.GetAllPokemonCards())
                {
                    if (pokemon.DamageCounters > 0)
                    {
                        pokemon.DealDamage(Amount, game, pokemon, true);
                    }
                }
            }
        }
    }
}
