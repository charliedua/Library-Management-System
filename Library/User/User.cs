using Library.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Library
{
    public class User : Entity, ISavable
    {
        /// <summary>
        /// The state of user.
        /// </summary>
        public UserState state;

        /// <summary>
        /// The account
        /// </summary>
        private UserAccount _account;

        public bool Saved { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public User(string name, int id) : base(name)
        {
            this.Inventory = new Inventory(this);

            COL_NAMES.AddRange(new string[] { "Permissions", "State" });
            Database database = new Database();
            _id = id;
            database.Dispose();
            Permissions = new List<Permissions>() { Library.Permissions.None };
            state = UserState.Idle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class from the database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="account">The account.</param>
        /// <param name="perms">The array of Permissions.</param>
        public User(string name, int id, UserAccount account, UserState state, List<Permissions> perms) : this(name, id)
        {
            // FIXME: save it after the account is created
            Permissions = perms;
            this.state = state;
            _account = account;
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
        public override string Details => (_hasAccount ? base.Details + string.Format("Username: {0}\n", Account.Username) : base.Details) + string.Format("Permissions: \n{0}", HumanReadablePermissions());

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permissions> Permissions { get; set; }

        /// <summary>
        /// The table name
        /// </summary>
        public const string TABLE_NAME = "Users";

        /// <summary>
        /// determines if the user has an account.
        /// </summary>
        public bool _hasAccount => Account != null;

        /// <summary>
        /// the inventory for the items in the library.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        public Inventory Inventory { get; set; }

        #region Authentication stuff

        public bool IsAuthenticated { get => state == UserState.LoggedIN; }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="Library.AlreadyHasAccountException"></exception>
        public void CreateAccount(string username, string password)
        {
            state = UserState.CreatingAccount;
            if (!_hasAccount)
            {
                _account = new UserAccount(username, password);
                state = UserState.LoggedOut;
            }
            else
            {
                state = UserState.LoggedOut;
                throw new AlreadyHasAccountException(this);
            }
        }

        /// <summary>
        /// Authenticates user with the specified username and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>status</returns>
        /// <exception cref="DoesNotHaveAccountException"></exception>
        public bool Login(string username, string password)
        {
            if (!_hasAccount && state == UserState.Idle)
            {
                throw new DoesNotHaveAccountException();
            }
            state = UserState.LoggedIN;
            return _account.VerifyPassword(username: username, pass: password);
        }

        /// <summary>
        /// Logouts this user.
        /// </summary>
        /// <returns>success status</returns>
        public bool Logout()
        {
            if (state == UserState.LoggedIN)
            {
                state = UserState.LoggedOut;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Authentication stuff

        #region Permission Stuff

        /// <summary>
        /// converts Int to permissions.
        /// </summary>
        /// <param name="num">The number.</param>
        public static List<Permissions> IntToPerm(int num)
        {
            // TODO: Comment this method.
            List<Permissions> permissions = new List<Permissions>();
            switch (num)
            {
                // CRD
                // 000
                case 0:
                    permissions.Add(Library.Permissions.None);
                    break;

                // CRD
                // 001
                case 1:
                    permissions.Add(Library.Permissions.Delete);
                    break;

                // CRD
                // 010
                case 2:
                    permissions.Add(Library.Permissions.Read);
                    break;

                // CRD
                // 011
                case 3:
                    permissions.Add(Library.Permissions.Read);
                    goto case 1;

                // CRD
                // 100
                case 4:
                    permissions.Add(Library.Permissions.Create);
                    break;

                // CRD
                // 101
                case 5:
                    permissions.Add(Library.Permissions.Delete);
                    goto case 4;

                // CRD
                // 110
                case 6:
                    permissions.Add(Library.Permissions.Create);
                    goto case 2;

                // CRD
                // 111
                case 7:
                    permissions.Add(Library.Permissions.Create);
                    goto case 3;

                default:
                    return null;
            }
            return permissions;
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

        /// <summary>
        /// Gives Human readable permissions.
        /// </summary>
        /// <returns></returns>
        public string HumanReadablePermissions()
        {
            string final = "";
            foreach (Permissions permission in Permissions)
            {
                final += "\t" + permission.ToString() + " \n";
            }
            return final;
        }

        /// <summary>
        /// Converts Permissions to int.
        /// </summary>
        protected static int PermToInt(List<Permissions> permissions)
        {
            int final = 0;
            foreach (var perm in permissions)
            {
                final += (int)perm;
            }
            return final;
        }

        #endregion Permission Stuff

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
                List<Permissions> Permissions = IntToPerm((int)(long)reader["Permissions"]);

                // gets the int from db and converts to state
                UserState state = (UserState)(int)(long)reader["State"];
                UserAccount _account = null;
                if (state != UserState.Idle && state != UserState.CreatingAccount)
                {
                    _account = UserAccount.Load(reader);
                }
                bool _hasAccount = _account != null;
                return new User(_name, _id, _account, state, Permissions);
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
            List<string> colvalues = new List<string>() { _id.ToString(), _name, PermToInt(Permissions).ToString(), ((int)state).ToString() };
            if (state != UserState.Idle && state != UserState.CreatingAccount)
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
            if (IsAuthenticated)
            {
                Inventory.Put(item);
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
            }
            else
            {
                throw new InventoryDoesntHaveItemException(item, this);
            }
        }

        #endregion Interaction with items
    }
}