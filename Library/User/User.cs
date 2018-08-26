using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace Library
{
    public class User : Entity
    {
        private UserAccount _account;

        private bool _hasAccount;
        private Inventory _inventory;
        private bool _isAuthenticated;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public User(string name, string identifier) : base(name, identifier)
        {
            _account = new UserAccount();
            _isAuthenticated = false;
            _hasAccount = false;
            Permissions = new List<Permissions>() { Library.Permissions.minimal };
        }

        [JsonConstructor]
        public User(string name, string identifier, UserAccount account, Permissions[] perms) : this(name, identifier)
        {
            Permissions = new List<Permissions>(perms);
            _account = account;
        }

        public UserAccount Account
        {
            get => _account;
            set => _account = value;
        }

        /// <summary>
        /// inventory of things the user has acquired.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        public Inventory Inventory
        {
            get => _inventory; set => _inventory = value;
        }

        public List<Permissions> Permissions { get; set; }

        /// <summary>
        /// Authenticates user with the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Authenticate(string password)
        {
            _isAuthenticated = _account.VerifyPassword(_identifier, password);
            return _isAuthenticated;
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="password">The password.</param>
        public void CreateAccount(string password)
        {
            _account = new UserAccount(_identifier, password);
        }

        /// <summary>
        /// Gives the permission to user.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        public bool GivePermission(Permissions permission)
        {
            Permissions.Add(permission);
            return true;
        }

        /// <summary>
        /// Determines whether the specified user has permission.
        /// </summary>
        /// <param name="perm">The permission.</param>
        /// <returns>
        ///   <c>true</c> if the specified user has permission; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPermission(Permissions perm)
        {
            return Permissions.Contains(perm);
        }
    }
}