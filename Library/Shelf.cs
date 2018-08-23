using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Shelf : NotissuableItem
    {
        private List<Book> _books;

        public List<Book> Books
        {
            get { return _books; }
            set { _books = value; }
        }

        public Shelf(string name, string _identifier) : base(name, _identifier)
        {
        }
    }
}