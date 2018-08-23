using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Library
{
    public class User : Entity
    {
        private UserAccount _account;

        private bool _isAuthenticated;
        private bool _hasAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public User(string name, string identifier) : base(name, identifier)
        {
            _account = null;
            _isAuthenticated = false;
            Permissions = new Permissions[] { Library.Permissions.minimal };
        }

        [JsonConstructor]
        public User(string name, string identifier, UserAccount account) : this(name, identifier)
        {
            _account = account;
        }

        public UserAccount Account
        {
            get => _account;
            set => _account = value;
        }

        public Permissions[] Permissions { get; set; }

        public void CreateAccount(string password)
        {
            _account = new UserAccount(_identifier, password);
        }

        public bool Authenticate(string password)
        {
            return _account.Authenticate(_identifier, password);
        }
    }
}