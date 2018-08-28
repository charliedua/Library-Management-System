using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// The interface for classes who can issue a libraryItem
    /// </summary>
    public interface ICanIssue
    {
        /// <summary>
        /// Gets or sets the number of items acquired.
        /// </summary>
        /// <value>
        /// The number of items acquired.
        /// </value>
        int NumberOfItemsAcquired { get; }

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        Inventory Inventory { get; set; }

        /// <summary>
        /// Acquires the resources.
        /// </summary>
        /// <returns></returns>
        bool Acquire(LibraryItem item);

        /// <summary>
        /// Releases the acquired resources.
        /// </summary>
        /// <returns></returns>
        bool Release(LibraryItem item);
    }
}