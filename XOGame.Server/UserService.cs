using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XOGame.Core;

namespace XOGame.Server
{
    public class UserService
    {
        private readonly List<UserModel> _users;

        public UserService()
        {
            _users = new List<UserModel>();
        }

        public int Login(string username, string password)
        {
            var user = _users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                //register
                user=new UserModel();
                user.SecretToken = GenerateNewSecretToken();
                user.Password = password;
                user.Username = username;
                _users.Add(user);
                return user.SecretToken;
            }

            user = _users.FirstOrDefault(x => x.Username == username && x.Password == password);
            if (user == null)
            {
                //invalid password
                return 0;
            }
            return user.SecretToken;
        }

        private int GenerateNewSecretToken()
        {
            int secretToken = RandomHelper.RandomInteger();
            while (_users.Any(x=>x.SecretToken==secretToken))
            {
                secretToken = RandomHelper.RandomInteger();
            }

            return secretToken;
        }

        public UserModel[] GetAllUsers()
        {
            return _users.ToArray();
        }

        public void UpdateState(StatePacket packet)
        {
            var user = _users.FirstOrDefault(x => x.SecretToken == packet.SecretToken);
            if (user!=null)
            {
                user.PositionX = packet.PositionX;
                user.PositionY = packet.PositionY;
            }
            else
            {
                //invalid secret token,someone using brute force maybe
            }
        }
    }
}