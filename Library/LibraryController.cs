using System;
using System.Collections.Generic;
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
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public List<User> Users { get => _users; }

        private void LoadAllEntities()
        {
            Database database = new Database();
            var UserReader = database.LoadReader("Users");
            while (UserReader.HasRows)
            {
                _users.Add(User.Load(UserReader));
            }
            UserReader.Close();
            var ItemsReader = database.LoadReader("Items");
            while (ItemsReader.HasRows)
            {
                _items.Add(LibraryItem.Load(ItemsReader));
            }
            ItemsReader.Close();
            database.Dispose();
        }
    }
}