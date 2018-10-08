using System.Collections.Generic;
using System.Linq;

namespace Library
{
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

        public static void Load(Database database, User user, LibraryController controller)
        {
            Inventory inventory = new Inventory(user);
            var reader = database.LoadReader("Orders", string.Format("UserID = {0}", user.ID.ToString()));
            while (reader.Read())
            {
                int ItemID = (int)(long)reader["ItemID"];
                inventory.Put((LibraryItem)controller.FindEntityByID(Entities.Item, ItemID));
            }
            user.Inventory = inventory;
        }
    }
}