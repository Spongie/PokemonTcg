using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class HealAllEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Heal all your pokemon";
            }
        }

        private int amount;
        private bool opponents;

        [DynamicInput("Amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Opponents", InputControl.Boolean)]
        public bool Opponents
        {
            get { return opponents; }
            set
            {
                opponents = value;
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
            var target = Opponents ? opponent : caster;

            foreach (var pokemon in target.GetAllPokemonCards())
            {
                pokemon.Heal(Amount, game);
            }
        }
    }
}
