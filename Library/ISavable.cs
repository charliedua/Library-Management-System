using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Library
{
    public interface ISavable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISavable"/> is saved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if saved; otherwise, <c>false</c>.
        /// </value>
        bool Saved { get; set; }

        /// <summary>
        /// Saves this instance to db.
        /// </summary>
        void Save();
    }
}