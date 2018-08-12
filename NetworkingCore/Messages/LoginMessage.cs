namespace NetworkingCore.Messages
{
    public class LoginMessage : GenericMessageData
    {
        public LoginMessage(string userName, string password)
        {
            TargetClass = "Server.Services.UserService";
            TargetMethod = "Login";
            Parameters = new[] { userName, password };
        }
    }
}
