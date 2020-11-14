using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class DiscardAttachedEnergyAndHeal : DataModel, IEffect
    {
        private int amountToDiscard;
        private int amountToHeal;
        private TargetingMode targetingMode;

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount to Heal")]
        public int AmountToHeal
        {
            get { return amountToHeal; }
            set
            {
                amountToHeal = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount to Discard")]
        public int AmountToDiscard
        {
            get { return amountToDiscard; }
            set
            {
                amountToDiscard = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Discard attached energy and heal";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            while (true)
            {
                PokemonCard target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

                if (target.AttachedEnergy.Count < amountToDiscard)
                {
                    continue;
                }

                if (target.AttachedEnergy.Count == AmountToDiscard)
                {
                    target.Owner.DiscardPile.AddRange(target.AttachedEnergy);
                    target.AttachedEnergy.Clear();
                }
                else
                {
                    var message = new PickFromListMessage(target.AttachedEnergy, AmountToDiscard).ToNetworkMessage(game.Id);
                    var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

                    foreach (var id in response.Cards)
                    {
                        var card = target.AttachedEnergy.First(x => x.Id.Equals(id));

                        target.AttachedEnergy.Remove(card);
                        target.Owner.DiscardPile.Add(card);
                    }
                }

                target.DamageCounters -= AmountToHeal;

                if (target.DamageCounters < 0)
                {
                    target.DamageCounters = 0;
                }

                break;
            }
        }
    }
}
