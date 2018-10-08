using Dapper;
using Library.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Library.Commands.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: AlreadyHasAccountException.cs                                  //
    ////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: App.config                                                     //
    ////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: Database.cs                                                    //
    ////////////////////////////////////////////////////////////////////////////////

    public class Database : IDisposable
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// The connection
        /// </summary>
        private IDbConnection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database" /> class.
        /// </summary>
        /// <param name="connStr">The connection string.</param>
        public Database(string connStr = "Data Source=.//Library.db;Version=3;")
        {
            _connectionString = connStr;
            conn = new SQLiteConnection(_connectionString);
        }

        /// <summary>
        /// The status of database server.
        /// </summary>
        private bool _connected => conn.State == ConnectionState.Open;

        /// <summary>
        /// initiates the connection to server.
        /// </summary>
        /// <returns>
        /// status of <c>connection</c>
        /// </returns>
        public bool Connect()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return true;
        }

        /// <summary>
        /// Closses the connection to the server.
        /// </summary>
        /// <returns>
        /// status of <c>connection</c>
        /// </returns>
        public bool Disconnect()
        {
            conn.Close();
            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            conn.Dispose();
        }

        /// <summary>
        /// Gets the last identifier.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public int GetLastInsertedID(string tableName)
        {
            Connect();
            IDbCommand cmd = conn.CreateCommand();
            string query = string.Format("select max(ID) from `{0}`", tableName);
            cmd.CommandText = query;
            int id;
            id = (int)((long)cmd.ExecuteScalar());
            Disconnect();
            return id;
        }

        /// <summary>
        /// Loads from the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public SQLiteDataReader LoadReader(string tableName, string whereClause = "1 = 1")
        {
            Connect();
            string query = string.Format("select * from `{0}` where {1}", tableName, whereClause);
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            return (SQLiteDataReader)cmd.ExecuteReader();
        }

        public int Delete(string tableName, int ID)
        {
            Connect();
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format("delete from `{0}` where `ID` = {1}", tableName, ID.ToString());
            int rowsAffected = cmd.ExecuteNonQuery();
            Disconnect();
            return rowsAffected;
        }

        /// <summary>
        /// Saves to the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="coloumnNames">The coloumn names.</param>
        /// <param name="values">The values.</param>
        public void Save(string tableName, List<string> coloumnNames, List<string> values)
        {
            Connect();
            string cols = AggreegateAllInStrArr(coloumnNames.ToArray(), with: '`');
            string valuesStr = AggreegateAllInStrArr(values.ToArray());
            string query = string.Format("insert into `{0}` ({1}) values ({2})", tableName, cols, valuesStr);
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Disconnect();
        }

        /// <summary>
        /// Aggreegates all in string array.
        /// </summary>
        /// <param name="arr">The string array.</param>
        /// <returns></returns>
        private string AggreegateAllInStrArr(string[] arr, char with = '\'')
        {
            string final = "";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr.Length > 0)
                {
                    if (int.TryParse(arr[i], out int b))
                    {
                        final += b.ToString();
                    }
                    else
                    {
                        final += with + arr[i] + with;
                    }
                    final += ", ";
                }
                else
                    break;
            }
            if (int.TryParse(arr[arr.Length - 1], out int a))
            {
                final += a.ToString();
            }
            else
            {
                final += with + arr[arr.Length - 1] + with;
            }
            return final;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: DoesNotHaveAccountException.cs                                 //
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class DoesNotHaveAccountException : Exception
    {
        public DoesNotHaveAccountException() : base("User doesn't have a account")
        {
        }

        public DoesNotHaveAccountException(string message) : base(message)
        {
        }

        public DoesNotHaveAccountException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DoesNotHaveAccountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: DoesNotHavePermissionException.cs                              //
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class DoesNotHavePermissionException : Exception
    {
        public DoesNotHavePermissionException(Permissions permissions) : this(string.Format("You don't have permission to {0} this instance.", permissions.ToString()))
        {
        }

        public DoesNotHavePermissionException(string message) : base(message)
        {
        }

        public DoesNotHavePermissionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DoesNotHavePermissionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: Entities.cs                                                    //
    ////////////////////////////////////////////////////////////////////////////////

    public enum Entities
    {
        User,
        Item
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: Entity.cs                                                      //
    ////////////////////////////////////////////////////////////////////////////////

    public abstract class Entity
    {
        protected int _id;
        protected string _name;

        /// <summary>
        /// The col names in the table
        /// </summary>
        protected readonly List<string> COL_NAMES = new List<string>() { "ID", "Name" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Entity(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// with complete params.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ident">The ident.</param>
        public Entity(string name, int ident) : this(name)
        {
            _id = ident;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        public virtual string Details
        {
            get
            {
                return string.Format("Name: {0} \nID: {1}\n", Name, ID);
            }
        }

        /// <summary>
        /// Verifies the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool AreYou(int id) => id == _id;
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: Inventory.cs                                                   //
    ////////////////////////////////////////////////////////////////////////////////

    public class Inventory : ISavable
    {
        private readonly List<LibraryItem> _items;

        private User User { get; set; }

        public int NumberOfItems { get => _items.Count; }
        public bool Saved { get; set; } = false;
        public bool IsEmpty { get => _items.Count == 0; }
        public bool IsChanged { get; private set; } = false;

        public Inventory(User user)
        {
            User = user;
            _items = new List<LibraryItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class From the database.
        /// </summary>
        /// <param name="items">The items.</param>
        public Inventory(List<LibraryItem> items, User user)
        {
            User = user;
            _items = items;
        }

        public bool Has(int id)
        {
            var a = from LibraryItem x in _items
                    where x.AreYou(id)
                    select x;
            return a.Count() > 0;
        }

        public bool Has(LibraryItem item)
        {
            return _items.Find(x => x == item) != null;
        }

        public void Put(LibraryItem item)
        {
            _items.Add(item);
            IsChanged = true;
        }

        public void Take(LibraryItem item)
        {
            if (Has(item.ID))
            {
                _items.Remove(item);
                IsChanged = true;
            }
        }

        public void Save()
        {
            using (Database database = new Database())
            {
                foreach (var item in _items)
                {
                    database.Save("Orders", new List<string>() { "UserID", "ItemID" }, new List<string>() { User.ID.ToString(), item.ID.ToString() });
                }
            }
        }

        public static Inventory Load(Database database, User user, LibraryController controller)
        {
            Inventory inventory = new Inventory(user);
            var reader = database.LoadReader("Orders", string.Format("UserID = {0}", user.ID.ToString()));
            while (reader.Read())
            {
                int ItemID = (int)(long)reader["ItemID"];
                inventory.Put((LibraryItem)controller.FindEntityByID(Entities.Item, ItemID));
            }
            return inventory;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: InventoryDoesntHaveItemException.cs                            //
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    internal class InventoryDoesntHaveItemException : Exception
    {
        public InventoryDoesntHaveItemException(LibraryItem item, User user) : base(string.Format("The Item with ID: [{0}] could not be located in the Inventory of the user with ID: [{1}].", item.ID, user.ID))
        {
        }

        public InventoryDoesntHaveItemException(string message) : base(message)
        {
        }

        public InventoryDoesntHaveItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InventoryDoesntHaveItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: ISavable.cs                                                    //
    ////////////////////////////////////////////////////////////////////////////////

    public interface ISavable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISavable"/> is saved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if saved; otherwise, <c>false</c>.
        /// </value>
        bool Saved { get; set; }

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        void Save();
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: LibraryController.cs                                           //
    ////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// The controller for the whole library
    /// </summary>
    public class LibraryController
    {
        public bool _authenticated = false;

        /// <summary>
        /// The items
        /// </summary>
        private readonly List<LibraryItem> _items = new List<LibraryItem>();

        /// <summary>
        /// The users
        /// </summary>
        private readonly List<User> _users = new List<User>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryController"/> class.
        /// </summary>
        public LibraryController()
        {
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<LibraryItem> Items { get => _items; }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public List<User> Users { get => _users; }

        /// <summary>
        /// Deletes the entity by identifier.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        public Entity DeleteEntityByID(Entities entities, int ID)
        {
            Entity temp;
            if (entities == Entities.User)
            {
                temp = _users.Find(x => x.ID == ID);
                _users.RemoveAll(x => x.ID == ID);
            }
            else
            {
                temp = _items.Find(x => x.ID == ID);
                _items.RemoveAll(x => x.ID == ID);
            }
            return temp;
        }

        /// <summary>
        /// Finds the user by identifier.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        public Entity FindEntityByID(Entities entities, int ID)
        {
            return entities == Entities.User ? _users.Find(x => x.ID == ID) : (Entity)_items.Find(x => x.ID == ID);
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User FindUserByUsername(string username)
        {
            return _users.Find(x => x._hasAccount ? x.Account.Username == username : false);
        }

        /// <summary>
        /// Gets the next new entity identifier.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public int GetNextNewEntityID(Entities entities)
        {
            int toReturn;
            using (Database database = new Database())
            {
                if (entities == Entities.User)
                {
                    toReturn = database.GetLastInsertedID(User.TABLE_NAME);
                    int max = toReturn;
                    foreach (var user in _users)
                    {
                        if (user.ID > toReturn)
                        {
                            toReturn = user.ID;
                        }
                    }
                }
                else
                {
                    toReturn = database.GetLastInsertedID(LibraryItem.TABLE_NAME);
                    foreach (var item in _items)
                    {
                        if (item.ID > toReturn)
                        {
                            toReturn = item.ID;
                        }
                    }
                }
            }
            return toReturn + 1;
        }

        /// <summary>
        /// Loads all entities.
        /// </summary>
        public void LoadAllEntities()
        {
            Database database = new Database();
            SQLiteDataReader UserReader = database.LoadReader(User.TABLE_NAME, "1");
            while (UserReader.HasRows)
            {
                User user = User.Load(UserReader);
                if (user != null)
                {
                    user.Saved = true;
                    _users.Add(user);
                }
            }
            UserReader.Close();
            database.Disconnect();
            SQLiteDataReader ItemsReader = database.LoadReader(LibraryItem.TABLE_NAME, "1");
            while (ItemsReader.HasRows)
            {
                LibraryItem item = LibraryItem.Load(ItemsReader);
                if (item != null)
                {
                    item.Saved = true;
                    _items.Add(item);
                }
            }
            ItemsReader.Close();
            database.Dispose();
        }

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            if (_authenticated)
            {
                return false;
            }
            User user = FindUserByUsername(username);
            bool isUservalid = false;
            if (user != null)
            {
                isUservalid = user.Login(username, password);
                if (isUservalid)
                {
                    CurrentUser = user;
                    _authenticated = true;
                }
            }
            return isUservalid;
        }

        public bool Logout()
        {
            _authenticated = !CurrentUser.Logout();
            return _authenticated;
        }

        /// <summary>
        /// Saves the unsaved.
        /// </summary>
        /// <param name="_entities">The entities.</param>
        /// <returns></returns>
        public int SaveTheUnsaved(Entities _entities)
        {
            int num = 0;
            List<ISavable> savables = _entities == Entities.User ? _users.ToList<ISavable>() : _items.ToList<ISavable>();
            foreach (ISavable savable in savables.FindAll(x => !x.Saved))
            {
                savable.Save();
                num++;
            }
            return num;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: LibraryItem.cs                                                 //
    ////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// The base Class for almost everything in this library.
    /// </summary>
    public class LibraryItem : Entity, ISavable
    {
        /// <summary>
        /// The table name
        /// </summary>
        public const string TABLE_NAME = "Items";

        public bool Saved { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public LibraryItem(string name, int id) : base(name)
        {
            Database database = new Database();
            _id = id;
            database.Dispose();
            Available = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public LibraryItem(int id, string name) : base(name, id)
        {
            Available = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LibraryItem"/> is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if available; otherwise, <c>false</c>.
        /// </value>
        public bool Available { get; set; }

        public override string Details
        {
            get
            {
                return base.Details + string.Format("Available: {0}\n", Available.ToString());
            }
        }

        #region Database Stuff

        /// <summary>
        /// Loads this instance from db.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>
        /// the item
        /// </returns>
        public static LibraryItem Load(SQLiteDataReader reader)
        {
            LibraryItem item = null;
            if (reader.HasRows && reader.Read())
            {
                int _id = (int)(long)reader["ID"];
                string _name = (string)reader["Name"];
                item = new LibraryItem(_id, _name);
            }
            return item;
        }

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        public void Save()
        {
            List<string> colVals = new List<string>() { _id.ToString(), _name };
            Database database = new Database();
            database.Save(TABLE_NAME, COL_NAMES, colVals);
            Saved = true;
        }

        #endregion Database Stuff
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: NonUniqueEntityException.cs                                    //
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    internal class NonUniqueEntityException : Exception
    {
        public NonUniqueEntityException(string name, string value) : base(string.Format("The UNIQUE constraint failed on {0}: value: {1}", name, value))
        {
        }

        public NonUniqueEntityException(string message) : base(message)
        {
        }

        public NonUniqueEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NonUniqueEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: packages.config                                                //
    ////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: Permissions.cs                                                 //
    ////////////////////////////////////////////////////////////////////////////////

    public enum Permissions
    {
        None = 0,
        Create = 4,
        Read = 2,
        Delete = 1,
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: User.cs                                                        //
    ////////////////////////////////////////////////////////////////////////////////

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
            if (perm == Library.Permissions.None) return true;
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
                item.Available = false;
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

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: UserAccount.cs                                                 //
    ////////////////////////////////////////////////////////////////////////////////

    public class UserAccount
    {
        private readonly string _password;
        private readonly string _username;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccount" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="fromdb">if set to <c>true</c> [fromdb].</param>
        /// <param name="encrypt">if set to <c>true</c> [encrypt].</param>
        /// <exception cref="NonUniqueEntityException">Username</exception>
        public UserAccount(string username, string password, bool fromdb = false, bool encrypt = true)
        {
            if (fromdb)
            {
                _username = username;
                _password = password;
            }
            else
            {
                if (PerformUniqueCheck(username))
                    _username = username;
                else
                    throw new NonUniqueEntityException("Username", username);
                _password = encrypt ? GetSha256Hash(password) : password;
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password => _password;

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public UserState State { get; private set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username => _username;

        #region Database stuff

        /// <summary>
        /// Performs the unique check on username in Users Table.
        /// </summary>
        /// <param name="username">The username.</param>
        public bool PerformUniqueCheck(string username)
        {
            Database database = new Database();
            SQLiteDataReader reader = database.LoadReader("Users", string.Format("`Username` = '{0}'", username));
            var temp = reader.HasRows;
            reader.Close();
            return !temp;
        }

        /// <summary>
        /// Loads account from the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>the account</returns>
        public static UserAccount Load(SQLiteDataReader reader)
        {
            string _username = (string)reader["Username"];

            // the encrypted password from the database
            string _password = (string)reader["Password"];

            return new UserAccount(_username, _password, fromdb: true, encrypt: false);
        }

        /// <summary>
        /// changes the colnames and colvalues to accomodate the account details.
        /// </summary>
        /// <param name="COL_NAMES">The col names.</param>
        /// <param name="colvalues">The colvalues.</param>
        public void Save(List<string> COL_NAMES, List<string> colvalues)
        {
            COL_NAMES.AddRange(new string[] { "Username", "Password" });
            colvalues.AddRange(new string[] { Username, Password });
        }

        #endregion Database stuff

        /// <summary>
        /// Verifies the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="pass">The pass.</param>
        /// <returns></returns>
        public bool VerifyPassword(string username, string pass)
        {
            return username == _username & VerifySha256Hash(pass, _password);
        }

        #region Encryption Stuff

        /// <summary>
        /// Gets the sha256 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static string GetSha256Hash(string input)
        {
            using (SHA256 Sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = Sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Verifies the sha256 hash against a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        private static bool VerifySha256Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetSha256Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        #endregion Encryption Stuff
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: UserNotAuthenticatedException.cs                               //
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    internal class UserNotAuthenticatedException : Exception
    {
        public UserNotAuthenticatedException(User user) : base(string.Format("The user with ID: {0} is not authenticated", user.ID))
        {
        }

        public UserNotAuthenticatedException(string message) : base(message)
        {
        }

        public UserNotAuthenticatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotAuthenticatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: UserState.cs                                                   //
    ////////////////////////////////////////////////////////////////////////////////

    public enum UserState
    {
        Idle,
        LoggedIN,
        LoggedOut,
        CreatingAccount
    }

    ////////////////////////////////////////////////////////////////////////////////
    //  Code from: Utility.cs                                                     //
    ////////////////////////////////////////////////////////////////////////////////
}

namespace Library.Exceptions
{
}