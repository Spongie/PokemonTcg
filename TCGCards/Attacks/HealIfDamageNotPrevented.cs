using CardEditor.Views;
using NetworkingCore.Messages;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class HealIfDamageNotPrevented : Attack
    {
        private int amountToHeal;
        private bool isMay;

        [DynamicInput("Yes/No question", InputControl.Boolean)]
        public bool IsMay
        {
            get { return isMay; }
            set 
            { 
                isMay = value;
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
            
        public override void OnDamageDealt(int amount, Player owner)
        {
            if (amount <= 0)
            {
                return;
            }

            if (isMay)
            {
                var activateMessage = new YesNoMessage { Message = Description }.ToNetworkMessage(owner.Id);
                var activateResponse = owner.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(activateMessage);

                if (activateResponse.AnsweredYes)
                {
                    owner.ActivePokemonCard.DamageCounters -= AmountToHeal;
                }
            }
            else
            {
                owner.ActivePokemonCard.DamageCounters -= AmountToHeal;
            }
        }
    }
}
