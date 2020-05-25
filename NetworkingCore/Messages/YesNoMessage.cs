namespace NetworkingCore.Messages
{
    public class YesNoMessage : AbstractNetworkMessage
    {
        public YesNoMessage()
        {
            MessageType = MessageTypes.YesNoMessage;
        }

        public string Message { get; set; }
        public bool AnsweredYes { get; set; }
    }
}
