using NetworkingCore;

namespace Server.Services
{
    public class PlayerService : IService
    {
        public void InitTypes()
        {
            
        }

        public int Login(string username, NetworkId playerId)
        {
            MasterServer.Instance.Clients[playerId].Name = username;
            return 0;
        }
    }
}
