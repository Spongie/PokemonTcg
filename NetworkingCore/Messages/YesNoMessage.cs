namespace NetworkingCore.Messages
{
    public class YesNoMessage : AbstractNetworkMessage
    {
        public string Message { get; set; }
        public bool AnsweredYes { get; set; }
    }
}
