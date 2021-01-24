using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DiscardAttachedEnergy : DataModel, IEffect
    {
        private int amount;
        private TargetingMode targetingMode;
        private EnergyTypes energyType;
        private bool allowUseWithoutTarget;
        private bool askYesNo;
        private string onlyWithLikeName;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();

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


        [DynamicInput("Cards to discard")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
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

        [DynamicInput("Allow use without target", InputControl.Boolean)]
        public bool AllowUseWithoutTarget
        {
            get { return allowUseWithoutTarget; }
            set
            {
                allowUseWithoutTarget = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only with name ")]
        public string OnlyWithNameLike
        {
            get { return onlyWithLikeName; }
            set
            {
                onlyWithLikeName = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Discard attached energy";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (AllowUseWithoutTarget || Amount == -1)
            {
                return true;
            }

            foreach (var pokemon in Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard))
            {
                if (pokemon.AttachedEnergy.Count >= 1)
                {
                    return true;
                }
            }

            game.GameLog.AddMessage($"Cannot cast because no pokemon with atleast 1 energy was found");
            return false;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (MayAbility && !game.AskYesNo(caster, "Discard attached energy?"))
            {
                return;
            }

            if (!CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            if (Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard, OnlyWithNameLike).All(x => x.AttachedEnergy.Count == 0))
            {
                return;
            }

            int cardsDiscarded = 0;

            while (true)
            {
                PokemonCard target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard, string.Empty, OnlyWithNameLike);

                if (target.AttachedEnergy.Count == 0)
                {
                    continue;
                }

                if (Amount == -1 || target.AttachedEnergy.Count <= Amount)
                {
                    while (target.AttachedEnergy.Count > 0)
                    {
                        target.DiscardEnergyCard(target.AttachedEnergy[0], game);
                        cardsDiscarded++;
                    }

                    target.AttachedEnergy.Clear();
                }
                else
                {
                    var message = new PickFromListMessage(target.AttachedEnergy.OfType<Card>().ToList(), Amount);
                    var ids = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id)).Cards;

                    var cards = target.AttachedEnergy.Where(x => ids.Contains(x.Id)).ToList();

                    foreach (var card in cards)
                    {
                        target.DiscardEnergyCard(card, game);
                        cardsDiscarded++;
                    }
                }

                break;
            }

            if (targetingMode == TargetingMode.Self || targetingMode == TargetingMode.YourActive || targetingMode == TargetingMode.YourBench || targetingMode == TargetingMode.YourPokemon)
            {
                game.LastDiscard = cardsDiscarded;
            }
        }
    }
}
