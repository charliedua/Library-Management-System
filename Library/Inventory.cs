using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Inventory
    {
        private readonly List<LibraryItem> _items;

        public Inventory()
        {
            _items = new List<LibraryItem>();
        }

        public bool Has(string id)
        {
            var a = from LibraryItem x in _items
                    where x.AreYou(id)
                    select x;
            return a.Count() > 0;
        }

        public void Put(LibraryItem item)
        {
            _items.Add(item);
        }

        public void Take(LibraryItem item)
        {
            if (Has(item.Identifier))
            {
            }
        }
    }
}