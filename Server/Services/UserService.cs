using DataLayer;
using DataLayer.Queries;
using Entities;
using NetworkingCore;
using System;
using System.Linq;

namespace Server.Services
{
    internal class UserService : IService
    {
        public BooleanResult Register(string userName, string password)
        {
            if (Database.Instance.Select(new SelectQuery<User>().AndEquals(nameof(User.UserName), userName)).Any())
            {
                return false;
            }

            Database.Instance.Insert(new User
            {
                UserName = userName,
                Password = Crypto.hashSHA512(password)
            });

            return true;
        }

        public BooleanResult Login(string userName, string password)
        {
            var user = Database.Instance.Select(new SelectQuery<User>().AndEquals(nameof(User.UserName), userName).Limit(1)).FirstOrDefault();

            var hashedPassword = Crypto.hashSHA512(password);

            var result = user != null && user.Password == hashedPassword;

            return result;
        }

        public void InitTypes()
        {
            var user = new User();
        }
    }
}
