using System;

namespace Library
{
    public class Book : IIssuable
    {
        private bool _acquired;
        private string _author;

        public Book(string name, string identifier)
        {
        }

        public bool Acquired { get => _acquired; set => _acquired = value; }

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
        /// Loads this instance from db.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LibraryItem Load()
        {
            Database<LibraryItem> database = new Database<LibraryItem>();
            //database.Load("Books", string.Format("{0} == {1}", this.))
            return null;
        }

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Save()
        {
            throw new NotImplementedException();
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