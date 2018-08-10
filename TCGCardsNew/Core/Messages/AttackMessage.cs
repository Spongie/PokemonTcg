using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class AttackMessage : AbstractNetworkMessage
    {
        public AttackMessage(Attack attack)
        {
            Attack = attack;
            messageType = MessageTypes.Attack;
        }

        public Attack Attack { get; set; }
    }
}
