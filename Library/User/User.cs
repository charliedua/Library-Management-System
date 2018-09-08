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
        /// determines if the user has an account.
        /// </summary>
        protected bool _hasAccount = false;

        /// <summary>
        /// The state of user.
        /// </summary>
        public UserState state;

        /// <summary>
        /// The is authenticated
        /// </summary>
        protected bool _isAuthenticated = false;

        /// <summary>
        /// the inventory for the items in the library.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        private Inventory Inventory { get; set; }

        /// <summary>
        /// The account
        /// </summary>
        private UserAccount _account;

        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class From the database.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public User(int identifier) : base(identifier)
        {
            state = UserState.Idle;
            COL_NAMES.AddRange(new string[] { "Permissions", "State" });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public User(string name) : base(name)
        {
            Database database = new Database();
            _id = database.GetLastInsertedID(TABLE_NAME) + 1;
            Permissions = new List<Permissions>() { Library.Permissions.None };
            state = UserState.Idle;
            COL_NAMES.AddRange(new string[] { "Permissions", "State" });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="account">The account.</param>
        /// <param name="perms">The array of Permissions.</param>
        public User(string name, int id, UserAccount account, UserState state, List<Permissions> perms) : this(name)
        {
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
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permissions> Permissions { get; set; }

        /// <summary>
        /// The table name
        /// </summary>
        protected override string TABLE_NAME => "Users";

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
                _hasAccount = true;
                state = UserState.LoggedOut;
            }
            else
            {
                state = UserState.LoggedOut;
                throw new AlreadyHasAccountException(this);
            }
        }

        /// <summary>
        /// Authenticates user with the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Authenticate(string password)
        {
            if (!_hasAccount && state != UserState.Idle)
            {
                throw new DoesNotHaveAccountException();
            }
            _isAuthenticated = _account.VerifyPassword(Account.Username, password);
            return _isAuthenticated;
        }

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
            return Permissions.Contains(perm);
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

        /// <summary>
        /// converts Int to permissions.
        /// </summary>
        /// <param name="num">The number.</param>
        protected static List<Permissions> IntToPerm(int num)
        {
            List<Permissions> permissions = new List<Permissions>();
            switch (num)
            {
                case 0:
                    permissions.Add(Library.Permissions.None);
                    break;

                case 1:
                    permissions.Add(Library.Permissions.Delete);
                    break;

                case 2:
                    permissions.Add(Library.Permissions.Read);
                    break;

                case 3:
                    permissions.Add(Library.Permissions.Read);
                    goto case 1;

                case 4:
                    permissions.Add(Library.Permissions.Create);
                    break;

                case 5:
                    permissions.Add(Library.Permissions.Delete);
                    goto case 4;

                case 6:
                    permissions.Add(Library.Permissions.Create);
                    goto case 2;
                case 7:
                    permissions.Add(Library.Permissions.Create);
                    goto case 6;
                default:
                    return null;
            }
            return permissions;
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
            if (reader.HasRows)
            {
                reader.Read();
                int _id = reader.GetInt32(0);
                string _name = reader.GetString(1);
                List<Permissions> perms = IntToPerm(reader.GetInt32(2));
                UserState state = (UserState)reader.GetInt32(3);
                return new User(_name, _id, new UserAccount(), state, perms);
            }
            return null;
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
            Account.Save(COL_NAMES, colvalues);
            Database database = new Database();
            database.Save(TABLE_NAME, COL_NAMES, colvalues);
            database.Dispose();
        }

        #endregion Database Stuff
    }
}