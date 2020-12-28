using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DamageEffect : DataModel, IEffect
    {
        private int amount;
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private bool coinFlip;
        private bool applyWeaknessResistance;
        private bool askYesNo;
        private EnergyTypes energyType;
        private int energyToDiscard;
        private bool useLastCoin;
        private bool checkTails;

        [DynamicInput("Ask Yes/No", InputControl.Boolean)]
        public bool MayAbility
        {
            get { return askYesNo; }
            set
            {
                askYesNo = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy to discard")]
        public int EnergyToDiscard
        {
            get { return energyToDiscard; }
            set
            {
                energyToDiscard = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

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

        [DynamicInput("Use last coin flip?", InputControl.Boolean)]
        public bool UseLastCoin
        {
            get { return useLastCoin; }
            set
            {
                useLastCoin = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Trigger on tails instead?", InputControl.Boolean)]
        public bool CheckTails
        {
            get { return checkTails; }
            set
            {
                checkTails = value;
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
            attachedTo.DealDamage(Amount, game, attachedTo, false);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, pokemonSource).Count == 0)
            {
                return;
            }

            string yesNoInfo = EnergyToDiscard > 0 ? "Discard energy card?" : "Deal damage to a benched pokemon?";

            if (askYesNo && !game.AskYesNo(caster, yesNoInfo))
            {
                return;
            }

            if (energyToDiscard > 0)
            {
                var choices = pokemonSource.AttachedEnergy
                    .Where(e => EnergyType == EnergyTypes.All || EnergyType == EnergyTypes.None || e.EnergyType == EnergyType)
                    .OfType<Card>()
                    .ToList();

                var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(choices, energyToDiscard).ToNetworkMessage(game.Id));

                foreach (var id in response.Cards)
                {
                    var card = game.Cards[id];
                    pokemonSource.DiscardEnergyCard((EnergyCard)card, game);
                }
            }

            var targetValue = CheckTails ? 0 : 1;
            var lastValue = game != null && game.LastCoinFlipResult ? 1 : 0;

            if (UseLastCoin && lastValue != targetValue)
            {
                return;
            }
            else if (CoinFlip && game.FlipCoins(1) != targetValue)
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

            if (target == null)
            {
                return;
            }

            int damage; 

            if (ApplyWeaknessResistance)
            {
                damage = DamageCalculator.GetDamageAfterWeaknessAndResistance(Amount, pokemonSource, target, null, game.FindResistanceModifier());
            }
            else
            {
                damage = Amount;
            }

            target.DealDamage(damage, game, pokemonSource, true);
        }
    }
}
