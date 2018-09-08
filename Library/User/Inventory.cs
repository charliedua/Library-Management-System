using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Inventory
    {
        private readonly List<LibraryItem> _items;

        public int NumberOfItems { get => _items.Count; }

        public Inventory()
        {
            _items = new List<LibraryItem>();
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