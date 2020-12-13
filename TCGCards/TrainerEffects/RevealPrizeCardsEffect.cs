using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class RevealPrizeCardsEffect : IEffect
    {
        public string EffectType
        {
            get
            {
                return "Reveal prize cards";
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
            foreach (var prizeCard in caster.PrizeCards)
            {
                prizeCard.IsRevealed = true;
            }

            foreach (var prizeCard in opponent.PrizeCards)
            {
                prizeCard.IsRevealed = true;
            }
        }
    }
}
