using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class HealEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private int amount;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();
        private bool opponentPicks;

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinflipConditional
        {
            get { return coinflipConditional; }
            set
            {
                coinflipConditional = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Heal amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Targeting type", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Opponent Picks", InputControl.Boolean)]
        public bool OpponentPicks
        {
            get { return opponentPicks; }
            set
            {
                opponentPicks = value;
                FirePropertyChanged();
            }
        }


        public string EffectType
        {
            get
            {
                return "Heal";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            if (!CoinflipConditional.IsOk(game, attachedTo.Owner))
            {
                return;
            }

            attachedTo.Heal(Amount, game);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var selector = OpponentPicks ? opponent : caster;
            var other = OpponentPicks ? caster : opponent;

            if (!CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            PokemonCard target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, selector, other, selector.ActivePokemonCard);

            target?.Heal(Amount, game);
        }
    }
}
