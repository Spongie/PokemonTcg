using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class ShuffleDeckEffect : DataModel, IEffect
    {
        private bool targetsOpponent;

        public string EffectType
        {
            get
            {
                return "Shuffle deck";
            }
        }

        [DynamicInput("Targets opponent", InputControl.Boolean)]
        public bool TargetsOpponent
        {
            get { return targetsOpponent; }
            set
            {
                targetsOpponent = value;
                FirePropertyChanged();
            }
        }


        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            var target = targetsOpponent ? game.GetOpponentOf(attachedTo.Owner) : attachedTo.Owner;
            target.Deck.Shuffle();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = TargetsOpponent ? opponent : caster;
            target.Deck.Shuffle();
        }
    }
}
