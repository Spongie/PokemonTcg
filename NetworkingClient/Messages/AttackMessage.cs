using TCGCards;

namespace NetworkingClient.Messages
{
    public struct AttackMessage
    {
        public AttackMessage(Attack attack)
        {
            Attack = attack;
        }

        public Attack Attack { get; set; }
    }
}
