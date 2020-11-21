using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class RevealHandsEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Reveal hands";
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
            opponent.RevealCards(caster.Hand);
            caster.RevealCards(opponent.Hand);
        }
    }
}
