using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class PickStatusMessage : AbstractNetworkMessage
    {
        public PickStatusMessage()
        {
            MessageType = MessageTypes.SelectStatus;
        }

        public bool CanParalyze { get; set; }
        public bool CanSleep { get; set; }
        public bool CanPoison { get; set; }
        public bool CanConfuse { get; set; }
        public bool CanBurn { get; set; }
    }
}
