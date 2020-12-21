using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageMultipleEffect : DataModel, IEffect
    {
        private int teamBenchDamage;
        private int enemyBenchDamage;
        private EnergyTypes onlyOfType;

        [DynamicInput("Damage all of type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes OnlyOfType
        {
            get { return onlyOfType; }
            set
            {
                onlyOfType = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage to your bench")]
        public int TeamBenchDamage
        {
            get { return teamBenchDamage; }
            set
            {
                teamBenchDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to opponents bench")]
        public int EnemyBenchDamage
        {
            get { return enemyBenchDamage; }
            set
            {
                enemyBenchDamage = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Damage multiple";
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
            if (EnemyBenchDamage > 0)
            {
                foreach (var pokemon in opponent.BenchedPokemon.ValidPokemonCards)
                {
                    if (OnlyOfType == EnergyTypes.All || OnlyOfType == EnergyTypes.None || pokemon.Type == OnlyOfType)
                    {
                        pokemon.DealDamage(EnemyBenchDamage, game, pokemonSource, true);
                    }
                }
            }
            if (TeamBenchDamage > 0)
            {
                foreach (var pokemon in caster.BenchedPokemon.ValidPokemonCards)
                {
                    if (OnlyOfType == EnergyTypes.All || OnlyOfType == EnergyTypes.None || pokemon.Type == OnlyOfType)
                    {
                        pokemon.DealDamage(TeamBenchDamage, game, pokemonSource, true);
                    }
                }
            }
        }
    }
}