using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageEffect : DataModel, IEffect
    {
        private int amount;
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private bool coinFlip;
        private bool applyWeaknessResistance;

        [DynamicInput("Damage amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target?", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Apply weakness resistance", InputControl.Boolean)]
        public bool ApplyWeaknessResistance
        {
            get { return applyWeaknessResistance; }
            set
            {
                applyWeaknessResistance = value;
                FirePropertyChanged();
            }
        }


        public string EffectType
        {
            get
            {
                return "Damage";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            attachedTo.DealDamage(Amount, game, attachedTo);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

            int damage; 

            if (ApplyWeaknessResistance)
            {
                damage = game.GetDamageAfterWeaknessAndResistance(Amount, pokemonSource, target, null);
            }
            else
            {
                damage = Amount;
            }

            target.DealDamage(damage, game, pokemonSource);
        }
    }
}
