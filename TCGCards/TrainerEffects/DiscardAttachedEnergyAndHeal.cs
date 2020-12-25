using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

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
            foreach (var pokemon in Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard))
            {
                if (pokemon.AttachedEnergy.Count >= AmountToDiscard)
                {
                    return true;
                }
            }

            game.GameLog.AddMessage($"Cannot cast because no pokemon with atleast {AmountToDiscard} energy was found");
            return false;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            while (true)
            {
                PokemonCard target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

                if (target == null)
                {
                    return;
                }

                if (target.AttachedEnergy.Count < amountToDiscard)
                {
                    continue;
                }

                if (target.AttachedEnergy.Count == AmountToDiscard)
                {
                    for (int i = 0; i < AmountToDiscard; i++)
                    {
                        target.DiscardEnergyCard(target.AttachedEnergy[0], game);
                    }
                    target.AttachedEnergy.Clear();
                }
                else
                {
                    var message = new PickFromListMessage(target.AttachedEnergy.OfType<Card>().ToList(), AmountToDiscard).ToNetworkMessage(game.Id);
                    var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

                    foreach (var id in response.Cards)
                    {
                        var card = (EnergyCard)game.Cards[id];

                        target.DiscardEnergyCard(card, game);
                    }
                }

                target.Heal(AmountToHeal, game);

                break;
            }
        }
    }
}
