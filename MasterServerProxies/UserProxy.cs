using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterServerProxies
{
    public class UserProxy : IUserService
    {
        private readonly NetworkPlayer networkPlayer;

        public UserProxy(NetworkPlayer networkPlayer)
        {
            this.networkPlayer = networkPlayer;
        }

        public bool Login(string userName, string password)
        {

            return true;
        }

        public void Register(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
