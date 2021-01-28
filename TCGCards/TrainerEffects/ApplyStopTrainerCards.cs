using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyStopTrainerCards : DataModel, IEffect
    {
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Stop trainer casting";
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
            if (!CoinFlipConditional.IsOk(game, caster))
            {
                return;
            }

            game.TemporaryPassiveAbilities.Add(new StopTrainerCastsAbility(caster.ActivePokemonCard)
            {
                LimitedByTime = true,
                TurnsLeft = 2,
                IsBuff = true
            });
        }
    }
}
