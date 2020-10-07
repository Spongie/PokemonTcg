using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class AttackMessage : AbstractNetworkMessage
    {
        public AttackMessage()
        {
            MessageType = MessageTypes.SelectedAttack;
        }

        public AttackMessage(Attack attack) :this()
        {
            Attack = attack;
        }

        public Attack Attack { get; set; }
    }
}
