using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Book : LibraryItem, IIssuable
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

        /// <summary>
        /// Gives this book.
        /// </summary>
        /// <returns></returns>
        public bool Give()
        {
            if (_acquired)
            {
                _acquired = false;
            }
            return _acquired;
        }

        public bool IsAvailable()
        {
            return _acquired;
        }

        /// <summary>
        /// Takes this book and set the status.
        /// </summary>
        /// <returns>the status of the take</returns>
        public bool Take()
        {
            _acquired = IsAvailable();
            return _acquired;
        }
    }
}