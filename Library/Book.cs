using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Book : LibraryItem
    {
        private bool _acquired;
        private string _author;

        public Book(string name, string identifier) : base(name, identifier)
        {
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public bool Give()
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable()
        {
            return _acquired;
        }

        public bool Take()
        {
            throw new NotImplementedException();
        }
    }
}