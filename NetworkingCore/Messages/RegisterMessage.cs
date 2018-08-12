namespace NetworkingCore.Messages
{
    public class RegisterMessage : GenericMessageData
    {
        public RegisterMessage(string userName, string password)
        {
            TargetClass = "Server.Services.UserService";
            TargetMethod = "Register";
            Parameters = new[] { userName, password };
        }
    }
}
