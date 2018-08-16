using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// The base Class for almost everything in this library.
    /// </summary>
    public abstract class LibraryItem
    {
        private bool _acquired;

        public LibraryItem(string name)
        {
            Name = name;
        }

        public bool Take()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To return or release the resource captured
        /// </summary>
        /// <returns>
        /// the status is it was possible or not
        /// </returns>
        public bool Give()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To or acquire the resource required.
        /// </summary>
        /// <returns>
        /// the status is it was possible or not.
        /// </returns>
        public bool IsAvailable()
        {
            return !_acquired;
        }

        /// <summary>
        /// this represents the name for the item
        /// </summary>
        /// <value>name of the item</value>
        public string Name { get; set; }
    }
}