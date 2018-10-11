using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

/// <summary>
/// The main namespace
/// </summary>
namespace Library
{
    /// <summary>
    /// The base Class for almost everything in this library.
    /// </summary>
    public class LibraryItem : Entity
    {
        /// <summary>
        /// The table name
        /// </summary>
        public const string TABLE_NAME = "Items";

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public LibraryItem(long id, string name) : base(name, Convert.ToInt32(id))
        {
            Available = true;
            COL_NAMES.AddRange(new string[] { "Available" });
        }

        public LibraryItem(long ID, string Name, long MaximumLoanPeriod, bool Available) : this(ID, Name)
        {
            this.MaximumLoanPeriod = Convert.ToInt32(MaximumLoanPeriod);
            this.Available = Available;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LibraryItem"/> is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if available; otherwise, <c>false</c>.
        /// </value>
        public bool Available { get; set; }

        public override string Details
        {
            get
            {
                return base.Details + string.Format("Available: {0}\nMaximum Loan Period (in Days) : {1}\n", Available.ToString(), MaximumLoanPeriod.ToString());
            }
        }

        public User IssuedBy { get; set; }
        public DateTime IssuedOn { get; set; }

        // days
        public int MaximumLoanPeriod { get; set; }
    }
}