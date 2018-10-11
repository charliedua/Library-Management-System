using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Inventory
    {
        private readonly List<LibraryItem> _items;

        private User User { get; set; }

        public int NumberOfItems { get => _items.Count; }
        public bool IsEmpty { get => _items.Count == 0; }

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
        }

        public void Take(LibraryItem item)
        {
            if (Has(item.ID))
            {
                _items.Remove(item);
            }
        }
    }
}