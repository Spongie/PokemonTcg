using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class RevealHandsEffect : DataModel, IEffect
    {
        private bool youReveal = true;
        private bool opponentReveal = true;

        public string EffectType
        {
            get
            {
                return "Reveal hands";
            }
        }

        [DynamicInput("You Reveals", InputControl.Boolean)]
        public bool YouReveal
        {
            get { return youReveal; }
            set
            {
                youReveal = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Opponent Reveals", InputControl.Boolean)]
        public bool OpponentReveal
        {
            get { return opponentReveal; }
            set
            {
                opponentReveal = value;
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
            if (OpponentReveal)
            {
                caster.RevealCards(opponent.Hand);
            }
            if (YouReveal)
            {
                opponent.RevealCards(caster.Hand);
            }
        }
    }
}
