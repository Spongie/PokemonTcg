﻿namespace NetworkingCore.Messages
{
    public class GenericMessageData : AbstractNetworkMessage
    {
        public GenericMessageData()
        {
            messageType = MessageTypes.Generic;
        }

        public string TargetClass { get; set; }
        public string TargetMethod { get; set; }
        public object[] Parameters { get; set; }
    }
}
