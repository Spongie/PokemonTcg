using System;
using System.Collections.Generic;
using System.Text;

namespace MasterServerProxies
{
    public interface IUserService
    {
        void Register(string userName, string password);
        bool Login(string userName, string password);
    }
}
