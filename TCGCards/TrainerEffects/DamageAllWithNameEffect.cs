using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageAllWithNameEffect : DataModel, IEffect
    {
        private string names;
        private int damage;

        public string EffectType
        {
            get
            {
                return "Damage all with name";
            }
        }

        [DynamicInput("Damage")]
        public int Damage
        {
            get { return damage; }
            set
            {
                damage = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Names to search for (Split with ;)")]
        public string Names
        {
            get { return names; }
            set
            {
                names = value;
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
            var names = Names.ToLower().Split(';');

            foreach (var pokemon in caster.GetAllPokemonCards())
            {
                if (names.Contains(pokemon.Name.ToLower()))
                {
                    pokemon.DealDamage(Damage, game, pokemonSource, false);
                }
            }

            foreach (var pokemon in opponent.GetAllPokemonCards())
            {
                if (names.Contains(pokemon.Name.ToLower()))
                {
                    pokemon.DealDamage(Damage, game, pokemonSource, false);
                }
            }
        }
    }
}
