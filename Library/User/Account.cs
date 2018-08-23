using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class UserAccount
    {
        private readonly string _password;
        private readonly string _username;

        public UserAccount(string username, string password)
        {
            _username = username;
            _password = Encoding.ASCII.GetString(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(password.ToCharArray())));
        }

        public bool Authenticate(string username, string pass)
        {
            pass = Encoding.ASCII.GetString(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(pass.ToCharArray())));
            return username == _username && pass == _password;
        }
    }
}