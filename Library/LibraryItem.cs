using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The main namespace
/// </summary>
namespace Library
{
    /// <summary>
    /// The base Class for almost everything in this library.
    /// </summary>
    public abstract class LibraryItem : Entity
    {
        /// <summary>
        /// the status of the item (available or not).
        /// </summary>
        private bool _acquired;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public LibraryItem(string name)
        {
            _acquired = false;
            Name = name;
        }

        /// <summary>
        /// To or acquire the resource required.
        /// </summary>
        /// <returns>
        /// the status is it was possible or not.
        /// </returns>
        public virtual bool Take()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To return or release the resource captured
        /// </summary>
        /// <returns>
        /// the status is it was possible or not
        /// </returns>
        public virtual bool Give()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether this instance is available.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
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