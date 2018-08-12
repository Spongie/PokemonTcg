using NetworkingCore;
using Server.DataLayer;
using Server.DataLayer.Queries;
using Server.Entities;
using System;
using System.Linq;

namespace Server.Services
{
    internal class UserService : IService
    {
        public bool Register(string userName, string password)
        {
            if (Database.Instance.Select(new SelectQuery<User>().AndEquals(nameof(User.UserName), userName)).Any())
            {
                return false;
            }

            Database.Instance.Insert(new User
            {
                UserName = userName,
                Password = Crypto.hashSHA512(password),
                RegisteredDate = DateTime.Now,
                LastLogin = DateTime.Now
            });

            return true;
        }

        public bool Login(string userName, string password)
        {
            var user = Database.Instance.Select(new SelectQuery<User>().AndEquals(nameof(User.UserName), userName).Limit(1)).FirstOrDefault();
            
            var hashedPassword = Crypto.hashSHA512(password);
            
            var result = user != null && user.Password == hashedPassword;
            
            if (user != null && result)
            {
                user.LastLogin = DateTime.Now;
                Database.Instance.Update(user);
            }

            Database.Instance.Insert(new LoginStatistics
            {
                UserName = userName,
                Password = hashedPassword,
                Time = DateTime.Now,
                Successful = result
            });

            return result;
        }
    }
}
