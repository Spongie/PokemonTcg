using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class LookAtOpponentHand : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Look at opponents hand";
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
            caster.RevealCards(opponent.Hand);
        }
    }
}
