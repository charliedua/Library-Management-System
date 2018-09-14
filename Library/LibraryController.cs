using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    /// <summary>
    /// The controller for the whole library
    /// </summary>
    public class LibraryController
    {
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
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<LibraryItem> Items { get => _items; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User CurrentUser { get; set; }

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
        public bool DeleteEntityByID(Entities entities, int ID)
        {
            return entities == Entities.User ? _users.RemoveAll(x => x.ID == ID) > 0 : _items.RemoveAll(x => x.ID == ID) > 0;
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
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            User user = FindUserByUsername(username);
            if (user != null)
            {
                return user.Login(username, password);
            }
            return false;
        }

        /// <summary>
        /// Loads all entities.
        /// </summary>
        public void LoadAllEntities()
        {
            Database database = new Database();
            SQLiteDataReader UserReader = database.LoadReader("Users", "1");
            while (UserReader.HasRows)
            {
                User user = User.Load(UserReader);
                if (user != null)
                {
                    _users.Add(user);
                }
            }
            UserReader.Close();
            database.Disconnect();
            SQLiteDataReader ItemsReader = database.LoadReader("Items", "1");
            while (ItemsReader.HasRows)
            {
                LibraryItem item = LibraryItem.Load(ItemsReader);
                if (item != null)
                {
                    _items.Add(LibraryItem.Load(ItemsReader));
                }
            }
            ItemsReader.Close();
            database.Dispose();
        }
    }
}