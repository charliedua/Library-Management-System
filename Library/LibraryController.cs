using Library.Utils;
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

        public bool CheckForUniquenessInUsername(string username)
        {
            return _users.Find(x => x.HasAccount ? username == x.Account.Username : false) == null;
        }

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
        /// Finds the name of the entity by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public List<Entity> FindEntityByName(string name)
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(Users.FindAll(x => x.Name == name));
            entities.AddRange(Items.FindAll(x => x.Name == name));
            return entities;
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User FindUserByUsername(string username)
        {
            return _users.Find(x => x.HasAccount ? x.Account.Username == username : false);
        }

        /// <summary>
        /// Gets the next new entity identifier.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public int GetNextNewEntityID(Entities entities)
        {
            int max = 0;
            if (entities == Entities.User)
            {
                foreach (var user in _users)
                {
                    if (user.ID > max)
                    {
                        max = user.ID;
                    }
                }
            }
            else
            {
                foreach (var item in _items)
                {
                    if (item.ID > max)
                    {
                        max = item.ID;
                    }
                }
            }
            return max + 1;
        }

        /// <summary>
        /// Loads all entities.
        /// </summary>
        public void LoadAllEntities()
        {
            _items.AddRange(Utility.LoadAllItems());
            _users.AddRange(Utility.LoadAllUsers());
            Utility.LoadInventory(_users, _items);
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
            if (user != null && user.HasAccount)
            {
                isUservalid = user.Account.Login(username, password);
                if (isUservalid)
                {
                    CurrentUser = user;
                    _authenticated = true;
                }
            }
            return isUservalid;
        }

        /// <summary>
        /// Logouts the user associated.
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            _authenticated = !CurrentUser.Account.Logout();
        }
    }
}