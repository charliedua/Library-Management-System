using Library.Exceptions;
using Library.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Library
{
    public class User : Entity
    {
        /// <summary>
        /// The table name
        /// </summary>
        public const string TABLE_NAME = "Users";

        /// <summary>
        /// The account
        /// </summary>
        private UserAccount _account = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public User(string name, int id) : base(name, id)
        {
            Inventory = new Inventory(this);
            COL_NAMES.AddRange(new string[] { "Permissions" });
            Permissions = new List<Permissions>() { Library.Permissions.None };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class from the database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="account">The account.</param>
        /// <param name="perms">The array of Permissions.</param>
        public User(string name, int id, UserAccount account, List<Permissions> perms) : this(name, id)
        {
            Permissions = perms;
            _account = account;
        }

        public User(long ID, string Name, long Permissions, long State, string Username, string Password, long MaximumItems) : base(Name, Convert.ToInt32(ID))
        {
            this.Permissions = Utility.IntToPerm(Convert.ToInt32(Permissions));
            CreateAccount(Username, Password, false);
            Account.State = (UserState)(int)State;
            MaxItems = Convert.ToInt32(MaximumItems);
            Inventory = new Inventory(this);
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public UserAccount Account { get => _account; set => _account = value; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        public override string Details => (HasAccount ? base.Details + string.Format("Username: {0}\n", Account.Username) : base.Details) + string.Format("Permissions: \n{0}", Utility.HumanReadablePermissions(Permissions));

        /// <summary>
        /// determines if the user has an account.
        /// </summary>
        public bool HasAccount => Account != null;

        /// <summary>
        /// the inventory for the items in the library.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// Gets or sets the maximum items.
        /// </summary>
        /// <value>
        /// The maximum items.
        /// </value>
        public int MaxItems { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permissions> Permissions { get; set; }

        #region Authentication stuff

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="AlreadyHasAccountException"></exception>
        /// <exception cref="Library.AlreadyHasAccountException"></exception>
        public void CreateAccount(string username, string password, bool encrypt = true)
        {
            if (!HasAccount)
            {
                _account = new UserAccount(username, password, this, encrypt);
            }
            else
            {
                throw new AlreadyHasAccountException(this);
            }
        }

        #endregion Authentication stuff

        #region Permission Stuff

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
            if (perm == Library.Permissions.None) return true;
            return Permissions.Contains(perm);
        }

        #endregion Permission Stuff

        /*

        #region Database Stuff

        /// <summary>
        /// Loads the next user from the database.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>the User</returns>
        public static User Load(SQLiteDataReader reader)
        {
            User user = null;
            if (reader.HasRows && reader.Read())
            {
                int _id = (int)(long)reader["ID"];
                string _name = (string)reader["Name"];

                // gets the int from db and converts to perm
                List<Permissions> Permissions = Utility.IntToPerm((int)(long)reader["Permissions"]);

                // gets the int from db and converts to state
                UserState state = (UserState)(int)(long)reader["State"];
                UserAccount _account = null;
                if (state != UserState.Idle)
                {
                    _account = UserAccount.Load(reader);
                }
                user = new User(_name, _id, _account, Permissions);
                _account.user = user;
                if (_account != null)
                {
                    _account.user = user;
                }
            }
            return user;
        }

        /// <summary>
        /// Saves to database.
        /// </summary>
        /// <param name="tablename">The tablename.</param>
        /// <param name="colnames">The colnames.</param>
        /// <param name="colvalues">The colvalues.</param>
        /// <returns></returns>
        public void Save()
        {
            List<string> colvalues = new List<string>() { _id.ToString(), _name, Utility.PermToInt(Permissions).ToString() };
            if (HasAccount)
            {
                Account.Save(COL_NAMES, colvalues);
            }
            Database database = new Database();
            database.Save(TABLE_NAME, COL_NAMES, colvalues);
            if ((Inventory.Saved || Inventory.IsEmpty) || Inventory.IsChanged)
            {
                Inventory.Save();
            }
            database.Dispose();
            Saved = true;
        }

        #endregion Database Stuff

    */

        #region Interaction with items

        /// <summary>
        /// Determines whether the specified identifier has item.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified identifier has item; otherwise, <c>false</c>.
        /// </returns>
        public bool HasItem(int ID)
        {
            return Inventory.Has(ID);
        }

        /// <summary>
        /// Issues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Issue(LibraryItem item)
        {
            if (_account.IsAuthenticated)
            {
                Inventory.Put(item);
                item.Available = false;
                item.IssuedBy = this;
                item.IssuedOn = DateTime.Now;
            }
            else
            {
                throw new UserNotAuthenticatedException(this);
            }
        }

        /// <summary>
        /// Returns the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="InventoryDoesntHaveItemException"></exception>
        public void Return(LibraryItem item)
        {
            if (Inventory.Has(item))
            {
                Inventory.Take(item);
                item.Available = true;
                item.IssuedBy = null;
            }
            else
            {
                throw new InventoryDoesntHaveItemException(item, this);
            }
        }

        #endregion Interaction with items
    }
}