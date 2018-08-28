using System.Collections.Generic;

namespace Library
{
    public class Shelf
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

        public override LibraryItem Load()
        {
            throw new System.NotImplementedException();
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}